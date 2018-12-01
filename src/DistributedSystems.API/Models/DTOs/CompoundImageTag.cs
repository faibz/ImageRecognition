using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class CompoundImageTag
    {
        public Guid CompoundImageId { get; set; }
        public string Tag { get; set; }
        public decimal Confidence { get; set; }

        public static explicit operator Shared.Models.CompoundImageTag(CompoundImageTag compoundImageTag)
        {
            return new Shared.Models.CompoundImageTag
            {
                CompoundImageId = compoundImageTag.CompoundImageId,
                Tag = compoundImageTag.Tag,
                Confidence = compoundImageTag.Confidence
            };
        }
    }
}