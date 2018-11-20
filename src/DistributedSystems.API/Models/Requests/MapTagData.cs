﻿using System;

namespace DistributedSystems.API.Models
{
    public class MapTagData : ImageTagData
    {
        public Guid MapId { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}