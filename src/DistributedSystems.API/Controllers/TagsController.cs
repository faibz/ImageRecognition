using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedSystems.API.Repositories;
using DistributedSystems.API.Services;
using DistributedSystems.API.Validators;
using DistributedSystems.Shared.Models;
using DistributedSystems.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsValidator _tagsValidator;
        private readonly IImagesService _imagesService;
        private readonly ITagsService _tagsService;
        private readonly ITagsRepository _tagsRepository;

        public TagsController(ITagsValidator tagsValidator, IImagesService imagesService, ITagsService tagsService, ITagsRepository tagsRepository, ICompoundImageTagsRepository compoundImageTagsRepository)
        {
            _tagsService = tagsService;
            _imagesService = imagesService;
            _tagsRepository = tagsRepository;
            _tagsValidator = tagsValidator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImageTags([FromBody] ImageTagData imageTagData)
        {
            if (!await _tagsService.ValidateTagDataKey(imageTagData)) return Unauthorized();

            await _tagsService.ProcessImageTags(imageTagData);
            await _imagesService.CompleteImageProcessing(imageTagData.ImageId);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapImagePartTags([FromBody] MapTagData mapTagData)
        {
            if (!await _tagsService.ValidateTagDataKey(mapTagData)) return Unauthorized();

            await _tagsValidator.ValidateImageTagData(mapTagData.TagData, mapTagData.ImageId);
            await _tagsService.ProcessImageTags(mapTagData);
            await _imagesService.CompleteImageProcessing(mapTagData.ImageId);
            await _tagsService.CheckForCompoundImageRequestsFromSingleMapImage(mapTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapCompoundImageTags([FromBody] CompoundImageTagData compoundImageTagData)
        {
            if (!await _tagsService.ValidateCompoundImageTagDataKey(compoundImageTagData)) return Unauthorized();

            await _tagsValidator.ValidateCompoundImageTagData(compoundImageTagData.Tags, compoundImageTagData.CompoundImageId);
            await _tagsService.ProcessCompoundImageTags(compoundImageTagData.CompoundImageId, compoundImageTagData.Tags);
            await _imagesService.CompleteCompoundImageProcessing(compoundImageTagData.CompoundImageId);
            await _tagsService.CheckForCompoundImageRequestFromCompoundImage(compoundImageTagData);

            return Ok();
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> GetTagsForImage(Guid imageId)
            => Ok(await _tagsRepository.GetTagsByImageId(imageId));

        [HttpGet("[action]/{mapId:Guid}")]
        public async Task<IActionResult> GetTagsForMapId(Guid mapId)
        {
            var tags = (List<Tag>) await _tagsRepository.GetTagsByMapId(mapId);
            tags.AddRange(await _tagsService.GetCompoundImageTagsByMapId(mapId));

            return Ok(tags);
        }
    }
}
