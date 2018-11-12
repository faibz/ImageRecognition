using System;
using System.Collections.Generic;

namespace DistributedSystems.API.Models
{
    public class CompoundImageTagData
    {
        public IList<MapTagData> MapTagData { get; set; }
        public string Key { get; set; }
    }
} 