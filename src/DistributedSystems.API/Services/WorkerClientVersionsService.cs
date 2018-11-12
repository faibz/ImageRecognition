using DistributedSystems.API.Models;
using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
{
    public interface IWorkerClientVersionsService
    {
        Task<bool> IsValidWorkerClientVersion(string clientVersion);
        Task<WorkerClientVersion> GetLatestWorkerClient();
    }

    public class WorkerClientVersionsService : IWorkerClientVersionsService
    {
        private readonly IWorkerClientVersionsRepository _workerVersionsRepository;

        public WorkerClientVersionsService(IWorkerClientVersionsRepository workerVersionsRepository)
        {
            _workerVersionsRepository = workerVersionsRepository;
        }

        public async Task<bool> IsValidWorkerClientVersion(string clientVersion)
        {
            return (await _workerVersionsRepository.GetWorkerByVersion(clientVersion)) == null;
        }

        public async Task<WorkerClientVersion> GetLatestWorkerClient()
        {
            return new WorkerClientVersion();
        }
    }
}