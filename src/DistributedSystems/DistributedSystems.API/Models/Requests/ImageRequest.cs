using Newtonsoft.Json;

namespace DistributedSystems.API.Models.Requests
{
    public class ImageRequest
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] Image { get; set; }
        //TODO: DOES THIS RELATE TO A BIGGER PHOTO? IF SO, MAP INFORMATION
    }
}