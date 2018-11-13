using System;

namespace DistributedSystems.API.Models
{
    public class MapImagePart
    {
        public MapImagePart() { }

        public MapImagePart(MapData mapData, Guid imageId)
        {
            MapId = mapData.MapId;
            ImageId = imageId;
            CoordinateX = mapData.Coordinates.X;
            CoordinateY = mapData.Coordinates.Y;
        }

        public Guid MapId { get; set; }
        public Guid ImageId { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public static explicit operator MapImagePart(DTOs.MapImagePart mapImagePart)
        {
            return new MapImagePart
            {
                MapId = mapImagePart.MapId,
                ImageId = mapImagePart.ImageId,
                CoordinateX = mapImagePart.CoordinateX,
                CoordinateY = mapImagePart.CoordinateY
            };
        }
    }
}