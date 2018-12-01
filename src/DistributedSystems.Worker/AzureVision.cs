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
        private ImageStitcher _imageStitcher;
        private static ComputerVisionClient _computerVision;
        private static readonly List<VisualFeatureTypes> _features = new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Tags
        };

        public AzureVision(IConfigurationRoot config)
        {
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
            var tagData = imageAnalysis.Tags.Select(tag => new Tag {Name = tag.Name, Confidence = (decimal)tag.Confidence }).ToList();

            return new ImageTagData
            {
                ImageId = image.Id,
                TagData = tagData,
                Key = image.ImageKey
            };
        }

        public async Task<CompoundImageTagData> ProcessCompoundImage(string message)
        {
            var keyedCompoundImage = JsonConvert.DeserializeObject<KeyedCompoundImage>(message);

            var stitchedImage = _imageStitcher.StitchImages(keyedCompoundImage);
            var imageAnalysis = await AnalyseMemoryStreamImage(stitchedImage);
            stitchedImage.Dispose();

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
    }
}
