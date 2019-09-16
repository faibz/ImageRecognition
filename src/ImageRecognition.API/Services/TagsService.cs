using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRecognition.API.Adapters;
using ImageRecognition.API.Repositories;
using ImageRecognition.API.Utils;
using ImageRecognition.Shared.Models;
using ImageRecognition.Shared.Models.Requests;

namespace ImageRecognition.API.Services
{
    public interface ITagsService
    {
        Task ProcessImageTags(ImageTagData imageTagData);
        Task ProcessCompoundImageTags(Guid compoundImageId, IList<Tag> tags);
        Task CheckForCompoundImageRequestsFromSingleMapImage(ImageTagData imageTagData);
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
        private readonly IMapsService _mapsService;
        private readonly ITagsAnalyser _tagsAnalyser;

        public TagsService(ITagsRepository tagsRepository, IImagesRepository imagesRepository, ICompoundImagesRepository compoundImagesRepository, ICompoundImageTagsRepository compoundImageTagsRepository, IQueueAdapter queueAdapter, ICompoundImageMappingsRepository compoundImageMappingsRepository, IImagesService imagesService, ITagsAnalyser tagsAnalyser, IMapsService mapsService)
        {
            _tagsRepository = tagsRepository;
            _imagesRepository = imagesRepository;
            _compoundImagesRepository = compoundImagesRepository;
            _compoundImageTagsRepository = compoundImageTagsRepository;
            _compoundImageMappingsRepository = compoundImageMappingsRepository;
            _imagesService = imagesService;
            _tagsAnalyser = tagsAnalyser;
            _mapsService = mapsService;
        }

        public async Task ProcessImageTags(ImageTagData imageTagData)
        {
            foreach (var tag in imageTagData.TagData)
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag,
                    imageTagData.MapId == Guid.Empty ? (Guid?) null : imageTagData.MapId);
        }
        public async Task ProcessCompoundImageTags(Guid compoundImageId, IList<Tag> tags)
        {
            foreach (var tag in tags)
                await _compoundImageTagsRepository.InsertCompoundImageTag(compoundImageId, tag);
        }

        public async Task CheckForCompoundImageRequestsFromSingleMapImage(ImageTagData imageTagData)
        {
            if (await _tagsAnalyser.AnalyseTagConfidence(imageTagData.TagData) == TagAnalysisAction.RequestCompoundImage && await _mapsService.VerifyMapCompletion(imageTagData.MapId))
                await _imagesService.CreateNewCompoundImage(imageTagData.MapId, new List<Guid> { imageTagData.ImageId });
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

            var keyParts = compoundImageTagData.Key.SplitInParts(32).ToList();

            var imageIds = await _compoundImageMappingsRepository.GetImageIdsByCompoundImageId(compoundImageTagData.CompoundImageId);

            var imageKeys = new List<string>();

            foreach (var imageId in imageIds)
            {
                imageKeys.Add(await _imagesRepository.GetImageKeyById(imageId));
            }

            return imageKeys.Count == keyParts.Count && imageKeys.All(keyParts.Contains);
        }

        public async Task<bool> ValidateTagDataKey(ImageTagData tagData)
        {
            if (string.IsNullOrEmpty(tagData.Key)) return false;

            var imageKey = await _imagesRepository.GetImageKeyById(tagData.ImageId);

            if (string.IsNullOrEmpty(imageKey)) return false;

            return tagData.Key == imageKey;
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