using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistributedSystems.API.Validators
{
    public interface ITagsValidator
    {
        Task<IList<Error>> ValidateTagData(MapTagData mapTagData);
    }

    public class TagsValidator : ITagsValidator
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsValidator(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task<IList<Error>> ValidateTagData(MapTagData mapTagData)
        {
            //TODO: THIS LOL
            throw new System.NotImplementedException();
        }
    }
}