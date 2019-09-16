using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRecognition.API.Repositories;
using ImageRecognition.Shared.Models;

namespace ImageRecognition.API.Utils
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

            if (mapImageParts.Count == map.ColumnCount * map.RowCount) return Guid.Empty;

            var nextImageId = Guid.Empty;
            var primaryMapPart = mapImageParts.First(imagePart => imagePart.ImageId == imageIds.First());
            var pastPrimaryMapParts = new List<MapImagePart>();

            while (nextImageId == Guid.Empty)
            {   
                //When creating a compound image, this code will look for the next map tile to add. If there are no remaining images to add, it will stop.
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
                    if (primaryMapPart.CoordinateY + 1 <= map.RowCount && pastPrimaryMapParts.All(part => part.CoordinateY == primaryMapPart.CoordinateY + 1 && part.CoordinateX == primaryMapPart.CoordinateX))
                    {
                        var upperMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY + 1);
                        if (imageIds.Any(imageId => imageId == upperMapPart.ImageId))
                        {
                            pastPrimaryMapParts.Add(primaryMapPart);
                            primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.First(imageId => imageId == upperMapPart.ImageId));
                        }
                    }
                    else if (primaryMapPart.CoordinateY - 1 >= 1 && pastPrimaryMapParts.All(part => part.CoordinateY == primaryMapPart.CoordinateY - 1 && part.CoordinateX == primaryMapPart.CoordinateX))
                    {
                        var lowerMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX, primaryMapPart.CoordinateY - 1);
                        if (imageIds.Any(imageId => imageId == lowerMapPart.ImageId))
                        {
                            pastPrimaryMapParts.Add(primaryMapPart);
                            primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.First(imageId => imageId == lowerMapPart.ImageId));
                        }
                    }
                    else if (primaryMapPart.CoordinateX + 1 <= map.ColumnCount && pastPrimaryMapParts.All(part => part.CoordinateX == primaryMapPart.CoordinateX + 1 && part.CoordinateY == primaryMapPart.CoordinateY))
                    {
                        var rightMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX + 1, primaryMapPart.CoordinateY);
                        if (imageIds.Any(imageId => imageId == rightMapPart.ImageId))
                        {
                            pastPrimaryMapParts.Add(primaryMapPart);
                            primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.First(imageId => imageId == rightMapPart.ImageId));
                        }
                    }
                    else if (primaryMapPart.CoordinateX - 1 >= 1 && pastPrimaryMapParts.All(part => part.CoordinateX == primaryMapPart.CoordinateX - 1 && part.CoordinateY == primaryMapPart.CoordinateY))
                    {
                        var leftMapPart = await _mapsRepository.GetMapImagePartByIdAndLocation(mapId, primaryMapPart.CoordinateX - 1, primaryMapPart.CoordinateY);
                        if (imageIds.Any(imageId => imageId == leftMapPart.ImageId))
                        {
                            pastPrimaryMapParts.Add(primaryMapPart);
                            primaryMapPart = await _mapsRepository.GetMapImagePartByImageId(imageIds.First(imageId => imageId == leftMapPart.ImageId));
                        }
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