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
                //TODO: plz make better
                if(primaryMapPart.CoordinateY + 1 <= map.RowCount && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX && imagePart.CoordinateY == primaryMapPart.CoordinateY + 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY + 1)).ImageId;
                }
                else if (primaryMapPart.CoordinateX + 1 <= map.ColumnCount && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX + 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX + 1, primaryMapPart.CoordinateY)).ImageId;
                }
                else if (primaryMapPart.CoordinateX - 1 >= 1 && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX - 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX - 1, primaryMapPart.CoordinateY)).ImageId;
                }
                else if (primaryMapPart.CoordinateY - 1 >= 1 && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX && imagePart.CoordinateY == primaryMapPart.CoordinateY - 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY - 1)).ImageId;
                }
                else if (primaryMapPart.CoordinateX + 1 <= map.ColumnCount && primaryMapPart.CoordinateY + 1 <= map.RowCount && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX + 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY + 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX + 1, primaryMapPart.CoordinateY + 1)).ImageId;
                }
                else if (primaryMapPart.CoordinateX - 1 >= 1 && primaryMapPart.CoordinateY + 1 <= map.RowCount && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX - 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY + 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX - 1, primaryMapPart.CoordinateY + 1)).ImageId;
                }
                else if (primaryMapPart.CoordinateX + 1 <= map.ColumnCount && primaryMapPart.CoordinateY - 1 >= 1 && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX + 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY - 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX + 1, primaryMapPart.CoordinateY - 1)).ImageId;
                }
                else if (primaryMapPart.CoordinateX - 1 >= 1 && primaryMapPart.CoordinateY - 1 >= 1 && !mapImageParts.Any(imagePart => imagePart.CoordinateX == primaryMapPart.CoordinateX - 1 && imagePart.CoordinateY == primaryMapPart.CoordinateY - 1))
                {
                    nextImageId = (await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX - 1, primaryMapPart.CoordinateY - 1)).ImageId;
                }
                else
                {
                    if (primaryMapPart.CoordinateY + 1 <= map.RowCount)
                    {
                        var upperMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY + 1);
                        if (imageIds.Any(imageId => imageId == upperMapPart.ImageId)) primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.Where(imageId => imageId == upperMapPart.ImageId).First());
                    }
                    else if (primaryMapPart.CoordinateY - 1 >= 1)
                    {
                        var lowerMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY - 1);
                        if (imageIds.Any(imageId => imageId == lowerMapPart.ImageId)) primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.Where(imageId => imageId == lowerMapPart.ImageId).First());
                    }
                    else if (primaryMapPart.CoordinateX + 1 <= map.ColumnCount)
                    {
                        var rightMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX + 1, primaryMapPart.CoordinateY);
                        if (imageIds.Any(imageId => imageId == rightMapPart.ImageId)) primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.Where(imageId => imageId == rightMapPart.ImageId).First());
                    }
                    else if (primaryMapPart.CoordinateX - 1 >= 1)
                    {
                        var leftMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX - 1, primaryMapPart.CoordinateY);
                        if (imageIds.Any(imageId => imageId == leftMapPart.ImageId)) primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.Where(imageId => imageId == leftMapPart.ImageId).First());
                    }
                    else
                    {
                        return Guid.Empty;
                    }
                }
            }

            return nextImageId;
        }
    }
}