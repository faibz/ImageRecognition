using System;
using DistributedSystems.API.Models;

namespace DistributedSystems.API.Services
{
    public class ImageStatusResult
    {
        public Guid ImageId { get; set; }
        public ImageStatus ImageStatus { get; set; }

        public static explicit operator ImageStatusResult(Image image)
        {
            return new ImageStatusResult
            {
                ImageId = image.Id,
                ImageStatus = image.Status
            };
        }
    }
}