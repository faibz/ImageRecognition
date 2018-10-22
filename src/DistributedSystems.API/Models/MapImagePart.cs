using System;

namespace DistributedSystems.API.Models
{
    public class MapImagePart
    {
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