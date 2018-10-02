using System.IO;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Requests;
using DistributedSystems.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitImage([FromBody] ImageRequest imageRequest)
        {
            var imgStream = new MemoryStream(imageRequest.Image);
            if (imgStream.Length > 4000000) return BadRequest("Image too large");

            if (!(await _imagesService.UploadImage(new Image(imageRequest), imgStream)).Success) return BadRequest();

            return Ok();
        }
    }
}
