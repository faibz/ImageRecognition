using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Services;
using DistributedSystems.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly IMapValidator _mapValidator;
        private readonly IMapsService _mapsService;

        public MapsController(IMapValidator mapValidator, IMapsService mapService)
        {
            _mapValidator = mapValidator;
            _mapsService = mapService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateImageMap(int columnCount, int rowCount)
        {
            var validationErrors = _mapValidator.ValidateCreateImageMapRequest(columnCount, rowCount);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var map = await _mapsService.CreateNewImageMap(columnCount, rowCount);
            
            return Ok(map);
        }
    }
}
