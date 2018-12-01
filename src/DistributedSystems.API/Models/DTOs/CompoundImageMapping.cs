using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class CompoundImageMapping
    {
        public Guid ImageId { get; set; }
        public Guid CompoundImageId { get; set; }

        public static explicit operator Shared.Models.CompoundImageMapping(CompoundImageMapping compoundImageMapping)
        {
            return new Shared.Models.CompoundImageMapping
            {
                ImageId = compoundImageMapping.ImageId,
                CompoundImageId = compoundImageMapping.CompoundImageId
            };
        }
    }
}