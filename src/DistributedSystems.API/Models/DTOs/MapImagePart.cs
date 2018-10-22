using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class MapImagePart
    {
        public Guid MapId { get; set; }
        public Guid ImageId { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

    }
}