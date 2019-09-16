using System.Threading.Tasks;
using ImageRecognition.API.Services;
using ImageRecognition.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionsController : ControllerBase
    {
        private readonly IWorkerClientVersionsService _workerVersionsService;

        public VersionsController(IWorkerClientVersionsService workerVersionsService)
        {
            _workerVersionsService = workerVersionsService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLatestWorkerClientVersion()
        {
            var workerClientversion = await _workerVersionsService.GetLatestWorkerClient();

            if (workerClientversion == null) return NotFound("There are currently no worker clients available.");

            return Ok(workerClientversion);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkerClientVersion(string clientVersion)
        {
            var workerClientVersion = await _workerVersionsService.GetWorkerClientVersion(clientVersion);

            if (workerClientVersion == null) return BadRequest("Invalid client version specified.");

            return Ok(workerClientVersion);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateWorkerClient([FromBody] WorkerClientVersionUpdateRequest workerClientVersionUpdate)
        {
            if (workerClientVersionUpdate == null) return BadRequest();

            if (!_workerVersionsService.ValidateUpdateKey(workerClientVersionUpdate.Key)) return Unauthorized();

            var workerClient = await _workerVersionsService.UpdateWorkerClient(workerClientVersionUpdate.ClientData);

            if (string.IsNullOrEmpty(workerClient.Location))
                return UnprocessableEntity("Failed to process worker client update.");

            return Ok(workerClient);
        }
    }
}