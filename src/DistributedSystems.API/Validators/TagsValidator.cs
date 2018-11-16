using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistributedSystems.API.Validators
{
    public interface ITagsValidator
    {
        Task<IList<Error>> ValidateMapTagData(IList<Tag> tag, Guid mapId);
    }

    public class TagsValidator : ITagsValidator
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsValidator(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task<IList<Error>> ValidateMapTagData(IList<Tag> tags, Guid mapId)
        {
            //TODO: THIS LOL
            throw new System.NotImplementedException();
        }
    }
}