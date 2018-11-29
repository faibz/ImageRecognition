using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DistributedSystems.Shared.Models;
using System.Collections.Generic;

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
            var originalImage = new Bitmap(file);
            var colCount = CalculateColRowCount(originalImage.Width);
            var rowCount = CalculateColRowCount(originalImage.Height);
            var adjustedImage = new Bitmap(originalImage, colCount * 500, rowCount * 500);

            var result = await _httpClient.GetAsync($"Maps/CreateImageMap?columnCount={colCount}&rowCount={rowCount}");
            var map = JsonConvert.DeserializeObject<Map>(await result.Content.ReadAsStringAsync());

            var tiles = new List<Bitmap>();

            using (var graphics = Graphics.FromImage(originalImage))
            {
                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var bmp = new Bitmap(500, 500);

                        graphics.DrawImage(bmp, new Rectangle(colIndex * 500, rowIndex * 500, 500, 500));

                        tiles.Add(bmp);
                    }
                }
            }


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
