using ImageRecognition.Shared.Utils;
using Newtonsoft.Json;

namespace ImageRecognition.Shared.Models
{
    public class WorkerClientVersionUpdateRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ClientData { get; set; }
        public string Key { get; set; }
    }
}

