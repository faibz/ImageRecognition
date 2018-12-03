﻿using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DistributedSystems.Shared.Models;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace DistributedSystems.Client
{
    public class ImageHelper
    {
        private readonly HttpClient _httpClient;

        public ImageHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool success, Guid mapId)> SendImageRequest(string file)
        {
            var originalImage = new Bitmap(file);
            var colCount = CalculateColRowCount(originalImage.Width);
            var rowCount = CalculateColRowCount(originalImage.Height);
            var adjustedImage = new Bitmap(originalImage, colCount * 500, rowCount * 500);

            var result = await _httpClient.GetAsync($"Maps/CreateImageMap?columnCount={colCount}&rowCount={rowCount}");
            var map = JsonConvert.DeserializeObject<Map>(await result.Content.ReadAsStringAsync());

            var tiles = new List<((int columnIndex, int rowIndex) coordinate, Bitmap bmp)>();

            for (var colIndex = 1; colIndex <= colCount; colIndex++)
            {
                for (var rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    tiles.Add(((colIndex, rowIndex),
                        adjustedImage.Clone(new Rectangle((colIndex - 1) * 500, (rowIndex - 1) * 500, 500, 500),
                            adjustedImage.PixelFormat)));
                }
            }

            var orderedTiles =  tiles.OrderByDescending(tile => tile.coordinate.rowIndex);
            var successful = true;

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

                var imageReq = new ImageRequest
                {
                    Image = imageData,
                    MapData = new MapData
                    {
                        MapId = map.Id,
                        Coordinates = new Coordinate(tile.coordinate.columnIndex, rowCount + 1 - tile.coordinate.rowIndex)
                    }
                };

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{_httpClient.BaseAddress}Images/SubmitImage"),
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(imageReq), Encoding.UTF8, "application/json")
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode) successful = false;
            }

            return (successful, map.Id);
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