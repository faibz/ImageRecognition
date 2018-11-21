using System;

namespace DistributedSystems.API.Models
{
    public class MapData
    {
        public Guid MapId { get; set; }
        public Coordinate Coordinates { get; set; }
    }
}