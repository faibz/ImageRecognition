using System;
using System.Drawing;

namespace DistributedSystems.API.Models
{
    public class MapData
    {
        public Guid MapId { get; set; }
        public Point Coordinates { get; set; }
    }
}