using System;

namespace DistributedSystems.API.Models.Requests
{
    public class MapTagData : ImageTagData
    {
        public Guid MapId { get; set; }
    }
}