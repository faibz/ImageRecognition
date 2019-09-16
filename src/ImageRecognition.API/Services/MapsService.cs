using System;
using System.Threading.Tasks;
using ImageRecognition.API.Repositories;
using ImageRecognition.Shared.Models;

namespace ImageRecognition.API.Services
{
    public interface IMapsService
    {
        Task<Shared.Models.Map> CreateNewImageMap(int columnCount, int rowCount);
        Task<Shared.Models.MapImagePart> AddImageToMap(MapData mapData, Guid imageId);
        Task<bool> VerifyMapCompletion(Guid mapId);
    }

    public class MapsService : IMapsService
    {
        private readonly IMapsRepository _mapsRepository;

        public MapsService(IMapsRepository mapsRepository)
        {
            _mapsRepository = mapsRepository;
        }

        public async Task<Shared.Models.Map> CreateNewImageMap(int columnCount, int rowCount) 
            => await _mapsRepository.InsertMap(new Shared.Models.Map(columnCount, rowCount));

        public async Task<Shared.Models.MapImagePart> AddImageToMap(MapData mapData, Guid imageId) 
            => await _mapsRepository.InsertMapImagePart(new Shared.Models.MapImagePart(mapData, imageId));

        public async Task<bool> VerifyMapCompletion(Guid mapId)
        {
            var map = await _mapsRepository.GetMapById(mapId);
            var mapTiles = map.ColumnCount * map.RowCount;

            return mapTiles == await _mapsRepository.MapImagePartsUploaded(mapId);
        }
    }
}