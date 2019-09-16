using System.Linq;
using System.Threading.Tasks;
using ImageRecognition.API.Services;
using ImageRecognition.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly IMapsValidator _mapsValidator;
        private readonly IMapsService _mapsService;

        public MapsController(IMapsValidator mapsValidator, IMapsService mapsService)
        {
            _mapsValidator = mapsValidator;
            _mapsService = mapsService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateImageMap(int columnCount, int rowCount)
        {
            var validationErrors = _mapsValidator.ValidateCreateImageMapRequest(columnCount, rowCount);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var map = await _mapsService.CreateNewImageMap(columnCount, rowCount);
            
            return Ok(map);
        }
    }
}
