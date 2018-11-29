using System;

namespace DistributedSystems.Shared.Models
{
    public class MapImagePart
    {
        public MapImagePart() { }

        public MapImagePart(MapData mapData, Guid imageId)
        {
            MapId = mapData.MapId;
            ImageId = imageId;
            CoordinateX = mapData.Coordinate.X;
            CoordinateY = mapData.Coordinate.Y;
        }

        public Guid MapId { get; set; }
        public Guid ImageId { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
    }
}