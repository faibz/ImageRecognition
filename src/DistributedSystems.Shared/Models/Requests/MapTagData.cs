using System;

namespace DistributedSystems.Shared.Models.Requests
{
    public class MapTagData : ImageTagData
    {
        public Guid MapId { get; set; }
    }
}