using System;
using System.Collections.Generic;
using DistributedSystems.API.Models;

namespace DistributedSystems.Worker
{
    public class CompoundImageTagData
    {
        //TODO: IT'S ONLY HERE TEMPORARILY
        public Guid CompoundImageId { get; set; }
        public Guid MapId { get; set; }
        public IList<Tag> Tags { get; set; }
        public string Key { get; set; }
    }
}
