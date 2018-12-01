using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class MapImagePart
    {
        public Guid MapId { get; set; }
        public Guid ImageId { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public static explicit operator Shared.Models.MapImagePart(MapImagePart mapImagePart)
        {
            return new Shared.Models.MapImagePart
            {
                MapId = mapImagePart.MapId,
                ImageId = mapImagePart.ImageId,
                CoordinateX = mapImagePart.CoordinateX,
                CoordinateY = mapImagePart.CoordinateY
            };
        }

    }
}