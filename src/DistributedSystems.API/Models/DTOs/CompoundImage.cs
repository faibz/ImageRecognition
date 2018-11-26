using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class CompoundImage
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}