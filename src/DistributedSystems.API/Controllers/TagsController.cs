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
        
        //TODO: security? API key? data so we know we can trust response? image hash? key per image?
        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImageTags([FromBody] ImageTagData imageTagData)
        {
            //SECURITY STUFF HERE. HOW CAN WE TRUST THIS DATA? MAYBE JUST ADD AUTH ATTRIBUTE AND SETUP IN STARTUP

            await _tagService.ProcessImageTags(imageTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapImagePartTags([FromBody] MapTagData mapTagData)
        {
            await _tagService.ProcessImageTags(mapTagData);

            return Ok();
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> GetTagsForImage(Guid imageId)
        {
            return Ok(await _tagsRepository.GetTagsByImageId(imageId));
        }
    }
}
