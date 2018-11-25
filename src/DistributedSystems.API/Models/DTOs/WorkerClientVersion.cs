using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class WorkerClientVersion
    {
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Location { get; set; }
        public string Hash { get; set; }
    }
}
