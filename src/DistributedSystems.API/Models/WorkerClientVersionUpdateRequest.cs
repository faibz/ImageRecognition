using DistributedSystems.API.Utils;
using Newtonsoft.Json;

namespace DistributedSystems.API.Models
{
    public class WorkerClientVersionUpdateRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ClientData { get; set; }
        public string Key { get; set; }
    }
}

