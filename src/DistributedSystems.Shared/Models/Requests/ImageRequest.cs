using DistributedSystems.Shared.Utils;
using Newtonsoft.Json;

namespace DistributedSystems.Shared.Models.Requests
{
    public class ImageRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] Image { get; set; }
        public MapData MapData { get; set; }
    }
}