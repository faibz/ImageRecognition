using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class ImageTag
    {
        public Guid ImageId { get; set; }
        public string Tag { get; set; }
        public decimal Confidence { get; set; }
        public Guid? MapId { get; set; }
    }
}
