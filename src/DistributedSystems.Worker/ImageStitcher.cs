using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using DistributedSystems.Shared.Models;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        public Bitmap StitchImages(KeyedCompoundImage keyedCompoundImage)
        {
            // Set width and height assumming that every image has equal dimensions.
            int width = 1000;
            int height = 1000;
            List<int> coordsX = new List<int>();
            List<int> coordsY = new List<int>();
            Bitmap compoundImage = null; // Final, stitched image

            // Store coordinates of each image.
            foreach (var image in keyedCompoundImage.Images)
            {
                coordsX.Add(image.Coordinate.X);
                coordsY.Add(image.Coordinate.Y);
            }
            coordsX.Sort();
            coordsY.Sort();

            // Calculate canvas dimensions using the images count on each axis.
            width  *= coordsX[coordsX.Count - 1] - coordsX[0] + 1;
            height *= coordsY[coordsY.Count - 1] - coordsY[0] + 1;
            compoundImage = new Bitmap(width, height); // Initialise canvas with the dimensions.

            // Get a graphics object from the image so we can draw on it.
            using (Graphics g = Graphics.FromImage(compoundImage))
            {
                g.Clear(Color.Transparent); // Set the background color.
                Bitmap bitmap;
                var offsetX = 0;
                var offsetY = 0;

                // Go through each image and draw it on the _compoundImage.
                foreach (var image in keyedCompoundImage.Images)
                {
                    bitmap = CreateBitmapFromUrl(image.Image.Location);
                    offsetX = bitmap.Width * image.Coordinate.X - bitmap.Width;
                    offsetY = height - (bitmap.Height * image.Coordinate.Y - bitmap.Height) - bitmap.Height;

                    g.DrawImage(bitmap, new Rectangle(offsetX, offsetY, bitmap.Width, bitmap.Height));

                    bitmap.Dispose();
                }
            }

            Stream compoundImageStream = new MemoryStream();
            compoundImage.Save(compoundImageStream, ImageFormat.Jpeg);

            // If stitched image is greater than 4MB in size...
            if (compoundImageStream.Length > 400000)
            {
                // ...gradually decrease the image dimensions until it is less than 4MB in size (Azure Vision's limit).
                do
                {
                    compoundImageStream.Position = 0; // Rewind the memory stream.
                    compoundImage = ResizeImage(new Bitmap(compoundImageStream), Convert.ToInt32(compoundImage.Width * 0.9), Convert.ToInt32(compoundImage.Height * 0.9));
                    compoundImageStream.Position = 0;
                    compoundImage.Save(compoundImageStream, ImageFormat.Jpeg);
                }
                while (compoundImageStream.Position > 400000);

                return compoundImage;
            }

            return compoundImage;
        }

        private Bitmap CreateBitmapFromUrl(string location)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(location);
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            return new Bitmap(responseStream);
        }

        // High-quality image resizing.
        private static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
