﻿using System;
using System.IO;
using System.Threading.Tasks;
using DistributedSystems.API.Adapters;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Results;
using DistributedSystems.API.Repositories;
using Microsoft.Extensions.Configuration;

namespace DistributedSystems.API.Services
{
    public interface IImagesService
    {
        Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream);
    }

    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IFileStorageAdapter _storageAdapter;
        private readonly IQueueAdapter _queueAdapter;

        public ImagesService(IConfiguration config, IImagesRepository imagesRepository, IFileStorageAdapter storageAdapter, IQueueAdapter queueAdapter)
        {
            _imagesRepository = imagesRepository;
            _queueAdapter = queueAdapter;
            _storageAdapter = storageAdapter;
        }

        public async Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream)
        {
            try
            {
                image.Location = await _storageAdapter.UploadImage(image, memoryStream);
                await _imagesRepository.InsertImage(image);
                image.Location = await _storageAdapter.GetImageUriWithKey(image.Id);
                await _queueAdapter.SendMessage(image);
            }
            catch (Exception)
            {
                return new UploadImageResult(false, null);
            }

            return new UploadImageResult(true, image);
        }
    }
}