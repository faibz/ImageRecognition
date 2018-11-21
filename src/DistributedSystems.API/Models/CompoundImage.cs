using System;

namespace DistributedSystems.API.Models
{
    public class CompoundImage
    {
        public CompoundImage()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid MapId { get; set; }

        public static explicit operator CompoundImage(DTOs.CompoundImage compoundImage)
        {
            return new CompoundImage
            {
                Id = compoundImage.Id,
                MapId = compoundImage.MapId
            };
        }
    }
}