using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DistributedSystems.Client.Models;

namespace DistributedSystems.Client
{
    public class ImageHelper
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public ImageHelper()
        {
            _httpClient.BaseAddress = new Uri("https://distsysimageapi.azurewebsites.net/api/"); // requests using this must have not have a '/' at the start of path
        }

        public async Task DoImageStuff(string file)
        {
            var image = new Bitmap(file);
            var columns = CalculateColRowCount(image.Width);
            var rows = CalculateColRowCount(image.Height);

            var result = await _httpClient.GetAsync($"Maps/CreateImageMap?columnCount={columns}&rowCount={rows}");
            var map = JsonConvert.DeserializeObject<Map>(await result.Content.ReadAsStringAsync());
        }

        private static int CalculateColRowCount(int imageDimension)
        {
            if (imageDimension <= 0) return -1;
            if (imageDimension < 500) return 1;

            var colCount = 1;

            do
            {
                ++colCount;
                imageDimension -= 500;
            } while (imageDimension >= 500);

            return colCount;
        }
    }
}
