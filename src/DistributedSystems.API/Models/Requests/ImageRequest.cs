using Newtonsoft.Json;

namespace DistributedSystems.API.Models.Requests
{
    public class ImageRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] Image { get; set; }
        public MapData MapData { get; set; }
    }
}