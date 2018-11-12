using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionsController : ControllerBase
    {
        private readonly IWorkerClientVersionsRepository _workerVersionsRepository;
        private readonly IWorkerClientVersionsService _workerVersionsService;

        public VersionsController(IWorkerClientVersionsRepository workerVersionsRepository, IWorkerClientVersionsService workerVersionsService)
        {
            _workerVersionsRepository = workerVersionsRepository;
            _workerVersionsService = workerVersionsService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLatestWorkerClientVersion()
            => Ok(await _workerVersionsService.GetLatestWorkerClient());

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkerClientVersion(string clientVersion)
        {
            var workerClientVersion = await _workerVersionsService.GetWorkerClientVersion(clientVersion);

            if (workerClientVersion == null) return BadRequest("Invalid Worker Client version specified.");

            return Ok(workerClientVersion);
        }
    }
}
