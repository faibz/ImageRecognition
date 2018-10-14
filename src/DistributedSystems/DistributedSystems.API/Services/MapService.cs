using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Services
{
    public interface IMapService
    {
        Task<Map> CreateNewImageMap(int columnCount, int rowCount);
    }

    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<Map> CreateNewImageMap(int columnCount, int rowCount) 
            => await _mapRepository.InsertMap(new Map(columnCount, rowCount)); //remove?
    }
}