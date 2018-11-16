using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DistributedSystems.API.Adapters;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Results;
using DistributedSystems.API.Repositories;
using DistributedSystems.API.Utils;

namespace DistributedSystems.API.Services
{
    public interface IImagesService
    {
        Task<UploadImageResult> UploadImage(MemoryStream memoryStream);
        Task CreateNewCompoundImageFromSingleImage(Guid mapId, Guid imageId);
    }

    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IFileStorageAdapter _storageAdapter;
        private readonly IQueueAdapter _queueAdapter;
        private readonly ICompoundImagesRepository _compoundImagesRepository;
        private readonly IMapsAnalyser _mapsAnalyser;

        public ImagesService(IImagesRepository imagesRepository, IFileStorageAdapter storageAdapter, IQueueAdapter queueAdapter, ICompoundImagesRepository compoundImagesRepository, IMapsAnalyser mapsAnalyser)
        {
            _imagesRepository = imagesRepository;
            _queueAdapter = queueAdapter;
            _storageAdapter = storageAdapter;
            _compoundImagesRepository = compoundImagesRepository;
            _mapsAnalyser = mapsAnalyser;
        }

        public async Task<UploadImageResult> UploadImage(MemoryStream memoryStream)
        {
            //TODO:
            // Probably should do:
            // If the storage isn't available:
            // - write all required information to a file so that it can be retried
            // If the DB isn't available:
            // - write all required information to a file so that it can be retried
            // If the service bus isn't available:
            // - write all required information to a file so that it can be retried

            var image = new Image();

            using (var md5 = MD5.Create())
            {
                image.ImageKey = BitConverter.ToString(md5.ComputeHash(memoryStream)).Replace("-", "").ToLower();
            }

            memoryStream.Position = 0;
            image.Location = await _storageAdapter.UploadImage(image.Id, memoryStream);

            if (string.IsNullOrEmpty(image.Location))
            {
                image.Status = ImageStatus.Errored;
                await _imagesRepository.InsertImage(image);

                //write to file

                return UploadFailureResult();
            }

            if (!await _imagesRepository.InsertImage(image))
            {
                //write to file

                return UploadFailureResult();
            }

            image.Location = await _storageAdapter.GetImageUriWithKey(image.Id);

            if (string.IsNullOrEmpty(image.Location))
            {
                await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.Errored);

                //write to file

                return UploadFailureResult();
            }

            if (!await _queueAdapter.SendMessage(image))
            {
                await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.Errored);

                //write to file

                return UploadFailureResult();
            }

            if (!await _imagesRepository.UpdateImageStatus(image.Id, ImageStatus.AwaitingProcessing))
            {
                return UploadFailureResult();
            }

            return UploadSuccessResult(image);
        }

        private UploadImageResult UploadSuccessResult(Image image)
            => new UploadImageResult(true, image);

        private UploadImageResult UploadFailureResult()
            => new UploadImageResult(false, null);

        public async Task CreateNewCompoundImageFromSingleImage(Guid mapId, Guid imageId)
        {
            var nextImageId = await _mapsAnalyser.SelectNextImageId(mapId, new List<Guid> { imageId });

            _compoundImagesRepository.InsertCompoundImage(new CompoundImage { MapId = mapId });
        }
    }
}