using System;
using System.Threading.Tasks;
using DistributedSystems.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateWorkerClient([FromBody] WorkerClientVersionUpdateRequest workerClientVersionUpdate)
        {
            if (!_workerVersionsService.ValidateUpdateKey(workerClientVersionUpdate.Key)) return Unauthorized();

            await _workerVersionsService.UpdateWorkerClient(workerClientVersionUpdate.ClientData);

            return Ok();
        }
    }
}

public class WorkerClientVersionUpdateRequest
{
    [JsonConverter(typeof(Base64FileJsonConverter))]
    public byte[] ClientData { get; set; }
    public string Key { get; set; }
}