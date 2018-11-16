using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedSystems.API.Utils
{
    public interface IMapsAnalyser
    {
        Task<Guid> SelectNextImageId(Guid mapId, IList<Guid> imageIds);
    }

    public class MapsAnalyser : IMapsAnalyser
    {
        private readonly IMapsRepository _mapsRepository;

        public MapsAnalyser(IMapsRepository mapsRepository)
        {
            _mapsRepository = mapsRepository;
        }

        public async Task<Guid> SelectNextImageId(Guid mapId, IList<Guid> imageIds)
        {
            var map = await _mapsRepository.GetMapById(mapId);

            var mapImageParts = new List<MapImagePart>();

            foreach (var imageId in imageIds)
            {
                mapImageParts.Add(await _mapsRepository.GetMapImagePartByImageId(imageId));
            }

            var nextImageId = Guid.Empty;
            var primaryMapPart = mapImageParts.First(imagePart => imagePart.ImageId == imageIds.First());

            while (nextImageId == Guid.Empty)
            {
                //if can choose location above, pick it
                if(primaryMapPart.CoordinateY + 1 <= map.RowCount && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX && imagePart.CoordinateY == primaryMapPart.CoordinateY + 1))
                {

                }
            }

            return nextImageId;
        }
    }
}