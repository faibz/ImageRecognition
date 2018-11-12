using System;

namespace DistributedSystems.API.Models
{
    public class WorkerClientVersion
    {
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Location { get; set; }
        public string Hash { get; set; }

        public static explicit operator WorkerClientVersion(DTOs.WorkerClientVersion clientVersion)
        {
            return new WorkerClientVersion
            {
                Version = clientVersion.Version,
                ReleaseDate = clientVersion.ReleaseDate,
                Location = clientVersion.Location,
                Hash = clientVersion.Hash
            };
        }
    }
}
