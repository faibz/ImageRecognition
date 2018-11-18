using System;
using DistributedSystems.API.Models;
using System.Threading.Tasks;
using DistributedSystems.API.Repositories;
using System.Linq;
using System.Collections.Generic;
using DistributedSystems.API.Adapters;

namespace DistributedSystems.API.Services
{
    public interface ITagsService
    {
        Task ProcessImageTags(ImageTagData imageTagData);
        Task ProcessCompoundImageTags(Guid compoundImageId, IList<Tag> tags);
        Task CheckForCompoundImageRequestsFromSingleMapImage(MapTagData mapTagData);
        Task CheckForCompoundImageRequestFromCompoundImage(CompoundImageTagData compoundImageTagData);
        Task<bool> ValidateTagDataKey(ImageTagData tagData);
        Task<bool> ValidateCompoundImageTagDataKey(CompoundImageTagData compoundImageTagData);
    }

    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IImagesRepository _imagesRepository;
        private readonly ICompoundImageTagsRepository _compoundImageTagsRepository;
        private readonly IQueueAdapter _queueAdapter;
        private readonly ICompoundImageMappingsRepository _compoundImageMappingsRepository;
        private readonly IImagesService _imagesService;

        public TagsService(ITagsRepository tagsRepository, IImagesRepository imagesRepository, ICompoundImageTagsRepository compoundImageTagsRepository, IQueueAdapter queueAdapter, ICompoundImageMappingsRepository compoundImageMappingsRepository, IImagesService imagesService)
        {
            _tagsRepository = tagsRepository;
            _imagesRepository = imagesRepository;
            _compoundImageTagsRepository = compoundImageTagsRepository;
            _queueAdapter = queueAdapter;
            _compoundImageMappingsRepository = compoundImageMappingsRepository;
            _imagesService = imagesService;

        }

        public async Task ProcessImageTags(ImageTagData imageTagData)
        {
            foreach (var tag in imageTagData.TagData)
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag,
                    imageTagData.GetType() == typeof(MapTagData) ? ((MapTagData) imageTagData).MapId : (Guid?) null);
        }
        public Task ProcessCompoundImageTags(Guid compoundImageId, IList<Tag> tags)
        {
            //TODO 
            throw new NotImplementedException();
        }

        public async Task CheckForCompoundImageRequestsFromSingleMapImage(MapTagData mapTagData)
        {
            if (mapTagData.TagData.Any(tag => tag.Confidence < 0.5m))
            {
                await _imagesService.CreateNewCompoundImage(mapTagData.MapId, new List<Guid> { mapTagData.ImageId });
            }
        }

        public Task CheckForCompoundImageRequestFromCompoundImage(CompoundImageTagData compoundImageTagData)
        {
            //if (compoundImageTagData.MapTagData.Any(tagData => tagData.TagData))
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateCompoundImageTagDataKey(CompoundImageTagData compoundImageTagData)
        {
            if (string.IsNullOrEmpty(compoundImageTagData.Key)) return false;

            var suppliedKey = compoundImageTagData.Key;
            var imageKey = string.Empty;

            var imageIds = await _compoundImageMappingsRepository.GetImageIdsByCompoundImageId(compoundImageTagData.CompoundImageId);

            foreach (var imageId in imageIds)
            {
                imageKey += await _imagesRepository.GetImageKeyById(imageId);
            }

            return suppliedKey == imageKey;
        }

        public async Task<bool> ValidateTagDataKey(ImageTagData tagData)
        {
            if (string.IsNullOrEmpty(tagData.Key)) return false;

            var imageKey = await _imagesRepository.GetImageKeyById(tagData.ImageId);

            return tagData.Key == imageKey;
        }


    }
}