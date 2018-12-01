using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Requests;
using DistributedSystems.API.Services;
using DistributedSystems.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;
        private readonly IImagesValidator _imageValidator;
        private readonly IMapsValidator _mapsValidator;
        private readonly IMapsService _mapsService;

        public ImagesController(IImagesService imagesService, IImagesValidator imagesValidator, IMapsValidator mapsValidator, IMapsService mapsService)
        {
            _imagesService = imagesService;
            _imageValidator = imagesValidator;
            _mapsValidator = mapsValidator;
            _mapsService = mapsService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImage([FromBody] ImageRequest imageRequest)
        {
            var validationErrors = (List<Error>)_imageValidator.ValidateImageRequest(imageRequest);
            if (imageRequest.MapData != null) validationErrors.AddRange(await _mapsValidator.ValidateMapEntry(imageRequest.MapData));
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var imgStream = new MemoryStream(imageRequest.Image);

            var uploadImageResult = await _imagesService.UploadImage(imgStream, imageRequest.MapData?.MapId ?? Guid.Empty);
            if (!uploadImageResult.Success) return BadRequest();

            if (imageRequest.MapData != null) await _mapsService.AddImageToMap(imageRequest.MapData, uploadImageResult.Image.Id);

            return Ok(new { ImageId = uploadImageResult.Image.Id, UploadedDate = uploadImageResult.Image.UploadedDate });
        }

        [HttpGet("[action]/{imageId:Guid}")]
        public async Task<IActionResult> ImageStatus(Guid imageId) 
            => Ok(await _imagesService.GetImageStatus(imageId));
    }
}
