using System;

namespace ImageRecognition.API.Models.DTOs
{
    public class CompoundImage
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }

        public static explicit operator Shared.Models.CompoundImage(CompoundImage compoundImage)
        {
            return new Shared.Models.CompoundImage
            {
                Id = compoundImage.Id,
                MapId = compoundImage.MapId,
                UploadedDate = compoundImage.UploadedDate,
                ProcessedDate = compoundImage.ProcessedDate ?? null
            };
        }
    }
}