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
    public class MapController : ControllerBase
    {
        private readonly IMapValidator _mapValidator;
        private readonly IMapService _mapService;

        public MapController(IMapValidator mapValidator, IMapService mapService)
        {
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
    }
}
