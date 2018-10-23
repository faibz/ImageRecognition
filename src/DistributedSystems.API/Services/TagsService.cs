using System;
using DistributedSystems.API.Models;
using System.Threading.Tasks;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Services
{
    public interface ITagsService
    {
        Task ProcessImageTags(ImageTagData imageTagData);
    }

    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsService(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task ProcessImageTags(ImageTagData imageTagData)
        {
            foreach (var tag in imageTagData.TagData)
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag,
                    imageTagData.GetType() == typeof(MapTagData) ? ((MapTagData) imageTagData).MapId : (Guid?) null);
        }
    }
}