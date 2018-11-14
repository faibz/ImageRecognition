using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistributedSystems.Worker
{
    public class ImageAnalyser
    {
        private readonly HttpClient _httpClient;
        private ImageStitcher _imageStitcher;

        public ImageAnalyser()
        {
            _httpClient = new HttpClient();
            _imageStitcher = new ImageStitcher();
        }

        public async Task<ImageTagData> ProcessSingleImage(string message)
        {
            var image = JsonConvert.DeserializeObject<Image>(message);
            var tags = await SendImageUrlToComputerVisionApiAsync(image.Location);

            var imageData = new ImageTagData
            {
                ImageId = image.Id,
                TagData = tags,
                Key = image.ImageKey
            };

            return imageData;
        }

        public async Task<Object> ProcessCompoundImage(string message)
        {
            var images = JsonConvert.DeserializeObject<CompoundImage>(message);
            //var imageDataList = new List<CompoundImageTagData>();
            //var imagesList = new List<string>();

            //foreach(var image in images.Images)
            //{
            //    var mapTagData = new CompoundImage
            //    {
            //        MapId = image.MapId,
            //        ImageId = image.ImageId,
            //        Key = images.ImageKey
            //    };
            //    imageDataList.Add(mapTagData);

            //    //DONE?: Add the images to a list/array that will be given to the ImageStitcher.
            //    imagesList.Add(image.Location);
            //}

            //DONE?: Call ImageStitcher method, pass the images list/array.
            var stitchedImage = _imageStitcher.StitchImages(images);

            //TODO: make a POST request with the stitched image -- the below method only supports URL.
            var tags = await SendImageUrlToComputerVisionApiAsync(stitchedImage);

            var imageData = new
            {
                //oldMapTagData = imagesList,
                newTags = tags
            };

            return imageData;
        }

        private async Task<Tag[]> SendImageUrlToComputerVisionApiAsync(string imageLocation)
        {
            var apiRequest = new HttpRequestMessage
            {
                RequestUri = new Uri("https://uksouth.api.cognitive.microsoft.com/vision/v2.0/tag"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(new { url = imageLocation }), Encoding.UTF8, "application/json")
            };
            apiRequest.Headers.Add("Ocp-Apim-Subscription-Key", "1080c5eef7c04852b1623cc582b0faaf");
            apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiResponse = await _httpClient.SendAsync(apiRequest);

            var analysedPicture = await apiResponse.Content.ReadAsStringAsync();

            var analysedPictureJson = JObject.Parse(analysedPicture);
            
            return ((JArray)analysedPictureJson["tags"]).ToObject<Tag[]>().ToList<Tag>();
        }
    }
}
