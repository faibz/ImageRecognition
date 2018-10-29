using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using Newtonsoft.Json;

namespace DistributedSystems.Worker
{
    public class ImageAnalyser
    {
        private readonly HttpClient _httpClient;
        private readonly Image _image;

        public ImageAnalyser(string message)
        {
            _httpClient = new HttpClient();
            _image = JsonConvert.DeserializeObject<Image>(message);
        }

        public async Task<string> SendImageUrlToComputerVisionApiAsync()
        {
            var apiRequest = new HttpRequestMessage
            {
                RequestUri = new Uri("https://uksouth.api.cognitive.microsoft.com/vision/v2.0/tag"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(new { url = _image.Location }), Encoding.UTF8, "application/json")
            };
            apiRequest.Headers.Add("Ocp-Apim-Subscription-Key", "1080c5eef7c04852b1623cc582b0faaf");
            apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiResponse = await _httpClient.SendAsync(apiRequest);

            return await apiResponse.Content.ReadAsStringAsync();
        }
    }
}
