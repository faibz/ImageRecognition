using DistributedSystems.API.Models;
using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
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
                await _tagsRepository.InsertImageTag(imageTagData.ImageId, tag);
        }
    }
}