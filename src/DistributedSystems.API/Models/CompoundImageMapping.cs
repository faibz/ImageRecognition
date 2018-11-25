using System;

namespace DistributedSystems.API.Models
{
    public class CompoundImageMapping
    {
        public Guid ImageId { get; set; }
        public Guid CompoundImageId { get; set; }

        public static explicit operator CompoundImageMapping(DTOs.CompoundImageMapping compoundImageMapping)
        {
            return new CompoundImageMapping
            {
                ImageId = compoundImageMapping.ImageId,
                CompoundImageId = compoundImageMapping.CompoundImageId
            };
        }
    }
}