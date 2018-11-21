using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Requests;
using DistributedSystems.API.Repositories;
using DistributedSystems.API.Services;
using DistributedSystems.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;
        private readonly ITagsRepository _tagsRepository;
        private readonly ITagsValidator _tagsValidator;

        public TagsController(ITagsService tagsService, ITagsRepository tagsRepository, ITagsValidator tagsValidator)
        {
            _tagsService = tagsService;
            _tagsRepository = tagsRepository;
            _tagsValidator = tagsValidator;
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImageTags([FromBody] ImageTagData imageTagData)
        {
            if (!await _tagsService.ValidateTagDataKey(imageTagData)) return Unauthorized();

            await _tagsService.ProcessImageTags(imageTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapImagePartTags([FromBody] MapTagData mapTagData)
        {
            if (!await _tagsService.ValidateTagDataKey(mapTagData)) return Unauthorized();

            await _tagsValidator.ValidateImageTagData(mapTagData.TagData, mapTagData.ImageId);
            await _tagsService.ProcessImageTags(mapTagData);
            await _tagsService.CheckForCompoundImageRequestsFromSingleMapImage(mapTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapCompoundImageTags([FromBody] CompoundImageTagData compoundImageTagData)
        {
            if (!await _tagsService.ValidateCompoundImageTagDataKey(compoundImageTagData)) return Unauthorized();

            await _tagsValidator.ValidateCompoundImageTagData(compoundImageTagData.Tags, compoundImageTagData.CompoundImageId);
            await _tagsService.ProcessCompoundImageTags(compoundImageTagData.CompoundImageId, compoundImageTagData.Tags);
            await _tagsService.CheckForCompoundImageRequestFromCompoundImage(compoundImageTagData);

            return Ok();
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> GetTagsForImage(Guid imageId)
            => Ok(await _tagsRepository.GetTagsByImageId(imageId));
    }
}
