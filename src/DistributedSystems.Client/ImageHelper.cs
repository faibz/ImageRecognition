using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DistributedSystems.Shared.Models;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

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

            var tiles = new List<((int columnIndex, int rowIndex) coordinate, Bitmap bmp)>();

            using (var graphics = Graphics.FromImage(adjustedImage))
            {
                for (var colIndex = 1; colIndex <= colCount; colIndex++)
                {
                    for (var rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                    {
                        var bmp = new Bitmap(500, 500);

                        graphics.DrawImage(bmp, new Rectangle(colIndex * 500, rowIndex * 500, 500, 500));

                        tiles.Add(((colIndex, rowIndex), bmp));
                    }
                }
            }

            var orderedTiles =  tiles.OrderByDescending(tuple => tuple.coordinate.rowIndex);

            foreach (var tile in orderedTiles)
            {
                string imageData;

                using (var memoryStream = new MemoryStream())
                {
                    tile.bmp.Save(memoryStream, ImageFormat.Jpeg);
                    memoryStream.Position = 0;
                    var base64String = Convert.ToBase64String(memoryStream.ToArray());
                    imageData = base64String;
                }

                var lx = new ImageRequest
                {
                    Image = imageData,
                    MapData = new MapData
                    {
                        MapId = map.Id,
                        Coordinate = new Coordinate(tile.coordinate.columnIndex, rowCount + 1 - tile.coordinate.rowIndex)
                    }
                };

                var response = await _httpClient.PostAsync("Images/SubmitImage", new StringContent(JsonConvert.SerializeObject(lx)));
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
