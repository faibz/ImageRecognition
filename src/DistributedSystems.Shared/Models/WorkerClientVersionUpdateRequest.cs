using DistributedSystems.Shared.Utils;
using Newtonsoft.Json;

namespace DistributedSystems.Shared.Models
{
    public class WorkerClientVersionUpdateRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ClientData { get; set; }
        public string Key { get; set; }
    }
}

