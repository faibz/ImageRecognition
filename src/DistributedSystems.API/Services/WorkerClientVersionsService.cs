using DistributedSystems.API.Models;
using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
{
    public interface IWorkerClientVersionsService
    {
        Task<WorkerClientVersion> GetWorkerClientVersion(string clientVersion);
        Task<WorkerClientVersion> GetLatestWorkerClient();
        Task<bool> ValidateUpdateKey(string updateKey);
        Task UpdateWorkerClient(byte[] clientData);
    }

    public class WorkerClientVersionsService : IWorkerClientVersionsService
    {
        private readonly IWorkerClientVersionsRepository _workerVersionsRepository;

        public WorkerClientVersionsService(IWorkerClientVersionsRepository workerVersionsRepository)
        {
            _workerVersionsRepository = workerVersionsRepository;
        }

        public async Task<WorkerClientVersion> GetWorkerClientVersion(string clientVersion)
            => await _workerVersionsRepository.GetWorkerByVersion(clientVersion);

        public async Task<WorkerClientVersion> GetLatestWorkerClient()
            => await _workerVersionsRepository.GetLatestWorkerClient();

        public async Task<bool> ValidateUpdateKey(string updateKey)
        {
            return true;
        }

        public async Task UpdateWorkerClient(byte[] clientData)
        {
            //TODO: 
            //storage adapter upload
            //database entry
        }
    }
}