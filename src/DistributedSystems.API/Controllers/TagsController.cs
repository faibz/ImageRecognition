using System;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;
using DistributedSystems.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagService;
        private readonly ITagsRepository _tagsRepository;

        public TagsController(ITagsService tagService, ITagsRepository tagsRepository)
        {
            _tagService = tagService;
            _tagsRepository = tagsRepository;
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImageTags([FromBody] ImageTagData imageTagData)
        {
            if (!await _tagService.ValidateTagDataKey(imageTagData)) return Unauthorized();

            await _tagService.ProcessImageTags(imageTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapImagePartTags([FromBody] MapTagData mapTagData)
        {
            //TODO: Add tag validator (?) to check whether a certain tag has already been entered for an image

            if (!await _tagService.ValidateTagDataKey(mapTagData)) return Unauthorized();

            await _tagService.ProcessImageTags(mapTagData);

            return Ok();
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> GetTagsForImage(Guid imageId)
            => Ok(await _tagsRepository.GetTagsByImageId(imageId));
    }
}
