using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetLatestWorkerClientInfo()
        {
            return Ok(await _workerVersionsRepository.GetLatestWorkerClientInfo());
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetLatestWorkerClient()
        {
            var lx = new { ReleaseDate = DateTime.UtcNow, Version = "1.0.22.4252.2", Hash = "bobmarley" };

            var client = await _workerService.GetLatestWorkerClient();

            var xx = new { Client = client, ReleaseDate = "xd", Version = "111111.11111", Hash = "bbbbbbbbb" };

            return Ok(xx);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWorkerClientVersion(string clientVersion)
        {
            if (!await _workerVersionsService.IsValidWorkerClientVersion(clientVersion)) BadRequest("Invalid WorkerClient version specified.");

            return Ok();
        }
    }
}
