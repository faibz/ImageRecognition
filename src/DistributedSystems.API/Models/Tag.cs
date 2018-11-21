using System;
using DistributedSystems.API.Models.DTOs;

namespace DistributedSystems.API.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public decimal Confidence { get; set; }

        public static explicit operator Tag(ImageTag imageTag)
        {
            return new Tag
            {
                Name = imageTag.Tag,
                Confidence = imageTag.Confidence
            };
        }

        public static explicit operator Tag(CompoundImageTag compoundImageTag)
        {
            return new Tag
            {
                Name = compoundImageTag.Tag,
                Confidence = compoundImageTag.Confidence
            };
        }
    }
}