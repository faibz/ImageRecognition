using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class CompoundImageTag
    {
        public Guid CompoundImageId { get; set; }
        public string Tag { get; set; }
        public decimal Confidence { get; set; }
    }
}