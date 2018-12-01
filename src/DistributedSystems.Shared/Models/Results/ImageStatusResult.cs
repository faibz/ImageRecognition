using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DistributedSystems.Shared.Models.Results
{
    public class ImageStatusResult
    {
        public Guid ImageId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ImageStatus ImageStatus { get; set; }
    }
}