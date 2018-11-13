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
    }
}