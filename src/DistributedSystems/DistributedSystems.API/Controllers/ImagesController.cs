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
        private readonly IImageValidator _imageValidator;
        private readonly IMapValidator _mapValidator;
        private readonly IMapService _mapService;

        public ImagesController(IImagesService imagesService, IImageValidator imageValidator, IMapValidator mapValidator, IMapService mapService)
        {
            _imagesService = imagesService;
            _imageValidator = imageValidator;
            _mapValidator = mapValidator;
            _mapService = mapService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateImageMap(int columnCount, int rowCount)
        {

            var validationErrors = _mapValidator.ValidateCreateImageMapRequest(columnCount, rowCount);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var map = await _mapService.CreateNewImageMap(columnCount, rowCount);
            
            return Ok(map);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImage([FromBody] ImageRequest imageRequest)
        {
            //map stuff

            var validationErrors = _imageValidator.ValidateImageRequest(imageRequest);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var imgStream = new MemoryStream(imageRequest.Image);

            var uploadImageResult = await _imagesService.UploadImage(new Image(imageRequest), imgStream);

            if (!uploadImageResult.Success) return BadRequest();

            return Ok();
        }
    }
}
