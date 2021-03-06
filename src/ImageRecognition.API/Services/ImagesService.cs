﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ImageRecognition.API.Adapters;
using ImageRecognition.API.Repositories;
using ImageRecognition.API.Utils;
using ImageRecognition.Shared.Models;
using ImageRecognition.Shared.Models.Requests;
using ImageRecognition.Shared.Models.Results;
using Microsoft.Extensions.Configuration;

namespace ImageRecognition.API.Services
{
    public interface IImagesService
    {
        Task<UploadImageResult> UploadImage(MemoryStream memoryStream, Guid mapId);
        Task CreateNewCompoundImage(Guid mapId, IList<Guid> imageIds);
        Task CompleteImageProcessing(Guid imageId);
        Task<ImageStatusResult> GetImageStatus(Guid imageId);
        Task CompleteCompoundImageProcessing(Guid compoundImageId);
    }

    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IFileStorageAdapter _storageAdapter;
        private readonly IQueueAdapter _queueAdapter;
        private readonly ICompoundImagesRepository _compoundImagesRepository;
        private readonly ICompoundImageMappingsRepository _compoundImageMappingsRepository;
        private readonly IMapsAnalyser _mapsAnalyser;
        private readonly IMapsRepository _mapsRepository;
        private readonly string _storageContainerName;

        public ImagesService(IImagesRepository imagesRepository, IFileStorageAdapter storageAdapter, IQueueAdapter queueAdapter, ICompoundImagesRepository compoundImagesRepository, ICompoundImageMappingsRepository compoundImageMappingsRepository, IMapsAnalyser mapsAnalyser, IMapsRepository mapsRepository, IConfiguration configuration)
        {
            _imagesRepository = imagesRepository;
            _queueAdapter = queueAdapter;
            _storageAdapter = storageAdapter;
            _compoundImagesRepository = compoundImagesRepository;
            _compoundImageMappingsRepository = compoundImageMappingsRepository;
            _mapsAnalyser = mapsAnalyser;
            _mapsRepository = mapsRepository;
            _storageContainerName = configuration.GetValue<string>("Azure:CloudBlobImageContainerName");
        }

        public async Task<UploadImageResult> UploadImage(MemoryStream memoryStream, Guid mapId)
        {
            var image = new Image();

            using (var md5 = MD5.Create())
            {
                image.ImageKey = BitConverter.ToString(md5.ComputeHash(memoryStream)).Replace("-", "").ToLower();
            }

            memoryStream.Position = 0;
            image.Location = await _storageAdapter.UploadFile($"{image.Id}.jpg", memoryStream, _storageContainerName);

            if (string.IsNullOrEmpty(image.Location))
            {
                image.Status = ImageStatus.Errored;
                await _imagesRepository.InsertImage(image);
                return UploadFailureResult();
            }

            if (!await _imagesRepository.InsertImage(image))
            {
                return UploadFailureResult();
            }

            image.Location = await _storageAdapter.GetFileUriWithKey($"{image.Id}.jpg", _storageContainerName);

            if (string.IsNullOrEmpty(image.Location))
            {
                await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.Errored);
                return UploadFailureResult();
            }

            if (!await _queueAdapter.SendMessage(new ImageProcessRequest(image, mapId), "single-image"))
            {
                await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.Errored);
                return UploadFailureResult();
            }

            if (!await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.AwaitingProcessing))
            {
                return UploadFailureResult();
            }

            return UploadSuccessResult(image);
        }

        public async Task CreateNewCompoundImage(Guid mapId, IList<Guid> imageIds)
        {
            var nextImageId = await _mapsAnalyser.SelectNextImageId(mapId, imageIds);

            if (await CompoundImageAlreadyExists(mapId, imageIds)) return;
            if (nextImageId == Guid.Empty) return;

            imageIds.Add(nextImageId);

            var compoundImage = new CompoundImage { MapId = mapId };
            await _compoundImagesRepository.InsertCompoundImage(compoundImage);

            foreach (var imageId in imageIds)
            {
                await _compoundImageMappingsRepository.InsertCompoundImageMapping(imageId, compoundImage.Id);
            }

            var queueCompoundImage = new KeyedCompoundImage
            {
                CompoundImageId = compoundImage.Id,
                ImageKey = await GetCompoundKeyFromImageIds(imageIds),
                MapId = mapId,
                Images = await GetCompoundImagePartsFromIds(imageIds)
            };

            await _queueAdapter.SendMessage(queueCompoundImage, "compound-image");
        }

        private async Task<bool> CompoundImageAlreadyExists(Guid mapId, IList<Guid> imageIds)
        {
            var compoundImagesForMap = await _compoundImagesRepository.GetByMapId(mapId);

            foreach (var compoundImage in compoundImagesForMap)
            {
                var imagesInCompoundImage = await _compoundImageMappingsRepository.GetImageIdsByCompoundImageId(compoundImage.Id);

                if (imagesInCompoundImage.All(imageIds.Contains)) return true;
            }

            return false;
        }

        private async Task<IList<CompoundImagePart>> GetCompoundImagePartsFromIds(IEnumerable<Guid> imageIds)
        {
            var compoundImageParts = new List<CompoundImagePart>();

            foreach (var imageId in imageIds)
            {
                compoundImageParts.Add(
                    new CompoundImagePart(await _mapsRepository.GetMapImagePartByImageId(imageId))
                    {
                        Image = {Location = await _storageAdapter.GetFileUriWithKey($"{imageId}.jpg", _storageContainerName)}
                    });
            }

            return compoundImageParts;
        }

        private async Task<string> GetCompoundKeyFromImageIds(IEnumerable<Guid> imageIds)
        {
            var key = "";

            foreach(var id in imageIds)
            {
                key += await _imagesRepository.GetImageKeyById(id);
            }

            return key;
        }

        public async Task CompleteImageProcessing(Guid imageId)
        {
            await _imagesRepository.UpdateImageProcessedDate(imageId, DateTime.UtcNow);
            await _imagesRepository.UpdateImageStatus(imageId, ImageStatus.Complete);
        }

        public async Task<ImageStatusResult> GetImageStatus(Guid imageId) 
            => (ImageStatusResult) await _imagesRepository.GetById(imageId);

        public async Task CompleteCompoundImageProcessing(Guid compoundImageId) 
            => await _compoundImagesRepository.UpdateCompoundImageProcessedDate(compoundImageId, DateTime.UtcNow);

        private UploadImageResult UploadSuccessResult(Image image)
            => new UploadImageResult(true, image);

        private UploadImageResult UploadFailureResult()
            => new UploadImageResult(false, null);
    }
}