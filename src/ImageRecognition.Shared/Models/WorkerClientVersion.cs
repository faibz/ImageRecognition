using System;

namespace ImageRecognition.Shared.Models
{
    public class WorkerClientVersion
    {
        public WorkerClientVersion()
        { }

        public WorkerClientVersion(int version)
        {
            Version = $"{version}";
            ReleaseDate = DateTime.UtcNow;
        }
        
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Location { get; set; }
        public string Hash { get; set; }
    }
}
