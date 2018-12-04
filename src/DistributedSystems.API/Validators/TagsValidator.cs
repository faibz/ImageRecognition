using DistributedSystems.API.Repositories;
using DistributedSystems.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedSystems.API.Validators
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
            RemoveLongTags(tags);
            RemoveMatchingTags(tags, await _tagsRepository.GetTagsByImageId(imageId));
        }

        public async Task ValidateCompoundImageTagData(IList<Tag> tags, Guid compoundImageId)
        {
            RemoveLongTags(tags);
            RemoveMatchingTags(tags, (await _compoundImageTagsRepository.GetTagsByCompoundImageId(compoundImageId))
                .Select(tag => (Tag) tag)
                .ToList());
        }

        private static void RemoveLongTags(IList<Tag> tags)
        {
            foreach (var tag in tags)
            {
                if (tag.Name.Length > 16) tags.Remove(tag);
            }
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