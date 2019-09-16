using System;
using ImageRecognition.Shared.Models.Results;
using Newtonsoft.Json;

namespace ImageRecognition.Shared.Models
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

        public static explicit operator ImageStatusResult(Image image)
        {
            return new ImageStatusResult
            {
                ImageId = image.Id,
                ImageStatus = image.Status
            };
        }
    }
}
