using System;

namespace DistributedSystems.API.Models
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

        public static explicit operator CompoundImage(DTOs.CompoundImage compoundImage)
        {
            return new CompoundImage
            {
                Id = compoundImage.Id,
                MapId = compoundImage.MapId,
                UploadedDate = compoundImage.UploadedDate,
                ProcessedDate = compoundImage.ProcessedDate ?? null
            };
        }
    }
}