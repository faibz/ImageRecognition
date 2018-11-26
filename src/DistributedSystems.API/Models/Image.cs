using System;
﻿using Newtonsoft.Json;

namespace DistributedSystems.API.Models
{
    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid();
            UploadedDate = DateTime.UtcNow;
        }

        public Image(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string Location { get; set; }
        [JsonIgnore]
        public DateTime UploadedDate { get; set; }
        [JsonIgnore]
        public DateTime? ProcessedDate { get; set; }
        [JsonIgnore]
        public ImageStatus Status { get; set; } = ImageStatus.UploadComplete;
        public string ImageKey { get; set; }

        public static explicit operator Image(DTOs.Image image)
        {
            return new Image
            {
                Id = image.Id,
                Location = image.Location,
                UploadedDate = image.UploadedDate,
                ProcessedDate = image.ProcessedDate,
                Status = image.Status,
                ImageKey = image.ImageKey
            };
        }
    }
}
