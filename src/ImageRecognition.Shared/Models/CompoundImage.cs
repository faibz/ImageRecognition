using System;

namespace ImageRecognition.Shared.Models
{
    public class CompoundImage
    {
        public CompoundImage(Guid id)
        {
            Id = Guid.NewGuid();
        }

        public CompoundImage()
        {
            Id = Guid.NewGuid();
            UploadedDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}