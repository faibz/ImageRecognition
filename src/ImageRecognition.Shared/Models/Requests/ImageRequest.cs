using ImageRecognition.Shared.Utils;
using Newtonsoft.Json;

namespace ImageRecognition.Shared.Models.Requests
{
    public class ImageRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] Image { get; set; }
        public MapData MapData { get; set; }
    }
}