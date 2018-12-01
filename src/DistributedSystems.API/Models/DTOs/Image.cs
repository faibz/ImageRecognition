using DistributedSystems.Shared.Models;
using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public ImageStatus Status { get; set; }
        public string ImageKey { get; set; }

        public static explicit operator Shared.Models.Image(Image image)
        {
            return new Shared.Models.Image
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
