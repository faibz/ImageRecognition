using System;

namespace ImageRecognition.API.Models.DTOs
{
    public class WorkerClientVersion
    {
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Location { get; set; }
        public string Hash { get; set; }

        public static explicit operator Shared.Models.WorkerClientVersion(WorkerClientVersion clientVersion)
        {
            return new Shared.Models.WorkerClientVersion
            {
                Version = clientVersion.Version,
                ReleaseDate = clientVersion.ReleaseDate,
                Location = clientVersion.Location,
                Hash = clientVersion.Hash
            };
        }
    }
}
