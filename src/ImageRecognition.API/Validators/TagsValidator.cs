using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRecognition.API.Repositories;
using ImageRecognition.Shared.Models;

namespace ImageRecognition.API.Validators
{
    public interface ITagsValidator
    {
        Task ValidateImageTagData(IList<Tag> tag, Guid imageId);
        Task ValidateCompoundImageTagData(IList<Tag> tags, Guid compoundImageId);
    }

    public class TagsValidator : ITagsValidator
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly ICompoundImageTagsRepository _compoundImageTagsRepository;

        public TagsValidator(ITagsRepository tagsRepository, ICompoundImageTagsRepository compoundImageTagsRepository)
        {
            _tagsRepository = tagsRepository;
            _compoundImageTagsRepository = compoundImageTagsRepository;
        }

        public async Task ValidateImageTagData(IList<Tag> tags, Guid imageId)
        {
            RemoveMatchingTags(tags, await _tagsRepository.GetTagsByImageId(imageId));
        }

        public async Task ValidateCompoundImageTagData(IList<Tag> tags, Guid compoundImageId)
        {
            RemoveMatchingTags(tags, (await _compoundImageTagsRepository.GetTagsByCompoundImageId(compoundImageId))
                .Select(tag => (Tag) tag)
                .ToList());
        }

        private static void RemoveMatchingTags(ICollection<Tag> primaryList, IEnumerable<Tag> comparisonList)
        {
            var existingTags = comparisonList.ToList();

            if (!existingTags.Any()) return;

            foreach (var existingTag in existingTags)
            {
                var matchingTag = primaryList.FirstOrDefault(tag => tag.Name == existingTag.Name);
                if (matchingTag != null) primaryList.Remove(matchingTag);
            }
        }
    }
}