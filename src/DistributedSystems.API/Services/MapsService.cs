using System;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Services
{
    public interface IMapsService
    {
        Task<Map> CreateNewImageMap(int columnCount, int rowCount);
        Task<MapImagePart> AddImageToMap(MapData mapData, Guid imageId);
    }

    public class MapsService : IMapsService
    {
        private readonly IMapsRepository _mapsRepository;

        public MapsService(IMapsRepository mapsRepository)
        {
            _mapsRepository = mapsRepository;
        }

        public async Task<Map> CreateNewImageMap(int columnCount, int rowCount) 
            => await _mapsRepository.InsertMap(new Map(columnCount, rowCount));

        public async Task<MapImagePart> AddImageToMap(MapData mapData, Guid imageId) 
            => await _mapsRepository.InsertMapImagePart(new MapImagePart(mapData, imageId));
    }
}