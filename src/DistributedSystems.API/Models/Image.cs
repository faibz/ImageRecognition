using System;
﻿using Newtonsoft.Json;

namespace DistributedSystems.API.Models
{
    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid();
            UploadedDate = DateTime.UtcNow;;
        }

        public Guid Id { get; }
        public string Location { get; set; }
        [JsonIgnore]
        public DateTime UploadedDate { get; set; }
        [JsonIgnore]
        public ImageStatus Status { get; set; } = ImageStatus.UploadComplete;
        public string ImageKey { get; set; }
    }
}
