using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
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
        private readonly ITagsService _tagService;
        private readonly ITagsRepository _tagsRepository;
        private readonly ITagsValidator _tagsValidator;

        public TagsController(ITagsService tagService, ITagsRepository tagsRepository, ITagsValidator tagsValidator)
        {
            _tagService = tagService;
            _tagsRepository = tagsRepository;
            _tagsValidator = tagsValidator;
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
            if (!await _tagService.ValidateTagDataKey(mapTagData)) return Unauthorized();

            var tagValidationErrors = await _tagsValidator.ValidateTagData(mapTagData);
            if (tagValidationErrors.Any()) return BadRequest(tagValidationErrors);

            await _tagService.ProcessImageTags(mapTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapCompoundImageTags([FromBody] CompoundImageTagData compoundImageTagData)
        {
            if (!await _tagService.ValidateCompoundImageTagDataKey(compoundImageTagData)) return Unauthorized();

            var validationErrors = new List<Error>();

            foreach (var mapThing in compoundImageTagData.MapTagData)
                validationErrors.AddRange(await _tagsValidator.ValidateTagData(mapThing));

            if (validationErrors.Any()) return BadRequest(validationErrors);

            foreach (var mapThing in compoundImageTagData.MapTagData)
                await _tagService.ProcessImageTags(mapThing);

            return Ok();
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> GetTagsForImage(Guid imageId)
            => Ok(await _tagsRepository.GetTagsByImageId(imageId));
    }
}
