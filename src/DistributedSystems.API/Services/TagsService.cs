using System;
using System.Linq;
using DistributedSystems.API.Models;
using System.Threading.Tasks;
using DistributedSystems.API.Repositories;
using System.Collections.Generic;
using DistributedSystems.API.Adapters;
using DistributedSystems.API.Models.Requests;
using DistributedSystems.API.Utils;

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
        Task<IList<Tag>> GetCompoundImageTagsByMapId(Guid mapId);
    }

    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IImagesRepository _imagesRepository;
        private readonly ICompoundImagesRepository _compoundImagesRepository;
        private readonly ICompoundImageTagsRepository _compoundImageTagsRepository;
        private readonly ICompoundImageMappingsRepository _compoundImageMappingsRepository;
        private readonly IImagesService _imagesService;
        private readonly ITagsAnalyser _tagsAnalyser;

        public TagsService(ITagsRepository tagsRepository, IImagesRepository imagesRepository, ICompoundImagesRepository compoundImagesRepository, ICompoundImageTagsRepository compoundImageTagsRepository, IQueueAdapter queueAdapter, ICompoundImageMappingsRepository compoundImageMappingsRepository, IImagesService imagesService, ITagsAnalyser tagsAnalyser)
        {
            _tagsRepository = tagsRepository;
            _imagesRepository = imagesRepository;
            _compoundImagesRepository = compoundImagesRepository;
            _compoundImageTagsRepository = compoundImageTagsRepository;
            _compoundImageMappingsRepository = compoundImageMappingsRepository;
            _imagesService = imagesService;
            _tagsAnalyser = tagsAnalyser;
        }

        public async Task ProcessImageTags(ImageTagData imageTagData)
        {
            foreach (var tag in imageTagData.TagData)
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag,
                    imageTagData.GetType() == typeof(MapTagData) ? ((MapTagData) imageTagData).MapId : (Guid?) null);
        }
        public async Task ProcessCompoundImageTags(Guid compoundImageId, IList<Tag> tags)
        {
            foreach (var tag in tags)
                await _compoundImageTagsRepository.InsertCompoundImageTag(compoundImageId, tag);
        }

        public async Task CheckForCompoundImageRequestsFromSingleMapImage(MapTagData mapTagData)
        {
            if (await _tagsAnalyser.AnalyseTagConfidence(mapTagData.TagData) == TagAnalysisAction.RequestCompoundImage)
                await _imagesService.CreateNewCompoundImage(mapTagData.MapId, new List<Guid> { mapTagData.ImageId });
        }

        public async Task CheckForCompoundImageRequestFromCompoundImage(CompoundImageTagData compoundImageTagData)
        {
            if (await _tagsAnalyser.AnalyseTagConfidence(compoundImageTagData.Tags) == TagAnalysisAction.RequestCompoundImage)
            {
                var images = await _compoundImageMappingsRepository.GetImageIdsByCompoundImageId(compoundImageTagData.CompoundImageId);
                await _imagesService.CreateNewCompoundImage(compoundImageTagData.MapId, images);
            }
        }

        public async Task<bool> ValidateCompoundImageTagDataKey(CompoundImageTagData compoundImageTagData)
        {
            if (string.IsNullOrEmpty(compoundImageTagData.Key)) return false;

            var suppliedKey = compoundImageTagData.Key;
            var imageKey = string.Empty;

            var imageIds = await _compoundImageMappingsRepository.GetImageIdsByCompoundImageId(compoundImageTagData.CompoundImageId);

            foreach (var imageId in imageIds)
                imageKey += await _imagesRepository.GetImageKeyById(imageId);

            return suppliedKey == imageKey;
        }

        public async Task<bool> ValidateTagDataKey(ImageTagData tagData)
        {
            if (string.IsNullOrEmpty(tagData.Key)) return false;

            return tagData.Key == await _imagesRepository.GetImageKeyById(tagData.ImageId);
        }

        public async Task<IList<Tag>> GetCompoundImageTagsByMapId(Guid mapId)
        {
            var compoundImagesUnderMap = await _compoundImagesRepository.GetByMapId(mapId);
            var tags = new List<Tag>();

            foreach (var compoundImage in compoundImagesUnderMap)
            {
                var tagsForCompoundImages = await _compoundImageTagsRepository.GetTagsByCompoundImageId(compoundImage.Id);
                
                tags.AddRange(tagsForCompoundImages.Select(tag => (Tag) tag));
            }

            return tags;
        }
    }
}