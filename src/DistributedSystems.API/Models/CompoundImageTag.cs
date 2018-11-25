using System;

namespace DistributedSystems.API.Models
{
    public class CompoundImageTag
    {
        public Guid CompoundImageId { get; set; }
        public string Tag { get; set; }
        public decimal Confidence { get; set; }

        public static explicit operator CompoundImageTag(DTOs.CompoundImageTag compoundImageTag)
        {
            return new CompoundImageTag
            {
                CompoundImageId = compoundImageTag.CompoundImageId,
                Tag = compoundImageTag.Tag,
                Confidence = compoundImageTag.Confidence
            };
        }
    }
}