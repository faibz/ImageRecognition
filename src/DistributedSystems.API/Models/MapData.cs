using System;

namespace DistributedSystems.API.Models
{
    public class MapData
    {
        public Guid MapId { get; set; }
        public Coordinate Coordinates { get; set; }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}