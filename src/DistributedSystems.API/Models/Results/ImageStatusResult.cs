using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DistributedSystems.API.Models.Results
{
    public class ImageStatusResult
    {
        public Guid ImageId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
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