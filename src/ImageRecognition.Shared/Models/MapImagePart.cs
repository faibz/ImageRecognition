using System;

namespace ImageRecognition.Shared.Models
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
    }
}