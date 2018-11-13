﻿using System;
using DistributedSystems.API.Models;
using System.Threading.Tasks;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Services
{
    public interface ITagsService
    {
        Task ProcessImageTags(ImageTagData imageTagData);
        Task<bool> ValidateTagDataKey(ImageTagData tagData);
    }

    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IImagesRepository _imagesRepository;

        public TagsService(ITagsRepository tagsRepository, IImagesRepository imagesRepository)
        {
            _tagsRepository = tagsRepository;
            _imagesRepository = imagesRepository;
        }

        public async Task ProcessImageTags(ImageTagData imageTagData)
        {
            foreach (var tag in imageTagData.TagData)
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag,
                    imageTagData.GetType() == typeof(MapTagData) ? ((MapTagData) imageTagData).MapId : (Guid?) null);
        }

        public async Task<bool> ValidateTagDataKey(ImageTagData tagData)
        {
            if (string.IsNullOrEmpty(tagData.Key)) return false;

            var imageKey = await _imagesRepository.GetImageKeyById(tagData.ImageId);

            return tagData.Key == imageKey;
        }
    }
}