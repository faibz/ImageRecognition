using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Requests;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistributedSystems.Worker
{
    public class AzureVision
    {
        //private readonly HttpClient _httpClient;
        private ImageStitcher _imageStitcher;
        private static ComputerVisionClient _computerVision;
        private static readonly List<VisualFeatureTypes> _features = new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Tags
        };

        public AzureVision(IConfigurationRoot config)
        {
            //_httpClient = new HttpClient();
            _imageStitcher = new ImageStitcher();

            _computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(config["AzureVisionSubscriptionKey"]),
                new System.Net.Http.DelegatingHandler[] { });
            _computerVision.Endpoint = config["AzureVisionEndpoint"];
        }

        public async Task<ImageTagData> ProcessSingleImage(string message)
        {
            var image = JsonConvert.DeserializeObject<DistributedSystems.API.Models.Image>(message);
            var imageAnalysis = await AnalyseRemoteImage(image.Location);
            var coolTags = imageAnalysis.Tags.Select(tag => new Tag {Name = tag.Name, Confidence = (decimal)tag.Confidence }).ToList();

            return new ImageTagData
            {
                ImageId = image.Id,
                TagData = coolTags,
                Key = image.ImageKey
            };
        }

        public async Task<CompoundImageTagData> ProcessCompoundImage(string message)
        {
            var keyedCompoundImage = JsonConvert.DeserializeObject<KeyedCompoundImage>(message);

            var stitchedImage = _imageStitcher.StitchImages(keyedCompoundImage);
            var imageAnalysis = await AnalyseMemoryStreamImage(stitchedImage);
            stitchedImage.Dispose();

            // TODO: Make sure the data is passed correctly.
            return new CompoundImageTagData
            {
                CompoundImageId = keyedCompoundImage.CompoundImageId,
                MapId = keyedCompoundImage.MapId,
                Tags = (IList<Tag>)imageAnalysis.Tags,
                Key = keyedCompoundImage.ImageKey
            };
        }

        private static async Task<ImageAnalysis> AnalyseRemoteImage(string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return null;
            }

            return await _computerVision.AnalyzeImageAsync(imageUrl, _features);
        }

        private static async Task<ImageAnalysis> AnalyseMemoryStreamImage(Bitmap image)
        {
            var imageStream = new MemoryStream();
            image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);

            return await _computerVision.AnalyzeImageInStreamAsync(imageStream, _features);
        }

        //private async Task<Tag[]> SendImageUrlToComputerVisionApiAsync(string imageLocation)
        //{
        //    var apiRequest = new HttpRequestMessage
        //    {
        //        RequestUri = new Uri("https://uksouth.api.cognitive.microsoft.com/vision/v2.0/tag"),
        //        Method = HttpMethod.Post,
        //        Content = new StringContent(JsonConvert.SerializeObject(new { url = imageLocation }), Encoding.UTF8, "application/json")
        //    };
        //    apiRequest.Headers.Add("Ocp-Apim-Subscription-Key", "1080c5eef7c04852b1623cc582b0faaf");
        //    apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var apiResponse = await _httpClient.SendAsync(apiRequest);
        //    var analysedPicture = await apiResponse.Content.ReadAsStringAsync();
        //    var analysedPictureJson = JObject.Parse(analysedPicture);

        //    return ((JArray)analysedPictureJson["tags"]).ToObject<Tag[]>().ToList<Tag>();
        //}
    }
}