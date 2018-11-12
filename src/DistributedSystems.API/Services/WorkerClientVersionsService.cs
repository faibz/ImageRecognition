using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
{
    public interface IWorkerClientVersionsService
    {
        Task<bool> IsValidWorkerClientVersion(string clientVersion);
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
    }
}