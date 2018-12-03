using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using DistributedSystems.Shared.Models;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        public Bitmap StitchImages(KeyedCompoundImage keyedCompoundImage)
        {
            List<int> _coordsX = new List<int>();
            List<int> _coordsY = new List<int>();
            Bitmap _compoundImage = null; // Final, stitched image

            // Get width and height assumming that every image has equal dimensions.
            //Bitmap _anyImage = new Bitmap(keyedCompoundImage.Images[0].Image.Location);
            //int _width = _anyImage.Width;
            //int _height = _anyImage.Height;
            //_anyImage.Dispose();

            int _width = 1000;
            int _height = 1000;

            // Store coordinates of each image.
            foreach (var image in keyedCompoundImage.Images)
            {
                _coordsX.Add(image.Coordinate.X);
                _coordsY.Add(image.Coordinate.Y);
            }

            //_coordsX.OrderBy(coord => coord);
            //_coordsY.OrderBy(coord => coord);
            _coordsX.Sort();
            _coordsY.Sort();

            // Calculate canvas dimensions using the images count on each axis.
            _width  *= _coordsX[_coordsX.Count - 1] - _coordsX[0] + 1;
            _height *= _coordsY[_coordsY.Count - 1] - _coordsY[0] + 1;
            _compoundImage = new Bitmap(_width, _height); // Initialise canvas with the dimensions.

            // Get a graphics object from the image so we can draw on it.
            using (Graphics g = Graphics.FromImage(_compoundImage))
            {
                g.Clear(Color.Transparent); // Set the background color.
                Bitmap _bitmap;
                var _offsetX = 0;
                var _offsetY = 0;

                // Go through each image and draw it on the _compoundImage.
                foreach (var image in keyedCompoundImage.Images)
                {
                    //_bitmap = new Bitmap(image.Image.Location);
                    _bitmap = CreateBitmapFromUrl(image.Image.Location);
                    _offsetX = _bitmap.Width * image.Coordinate.X - _bitmap.Width;
                    _offsetY = _height - (_bitmap.Height * image.Coordinate.Y - _bitmap.Height) - _bitmap.Height;

                    g.DrawImage(_bitmap, new Rectangle(_offsetX, _offsetY, _bitmap.Width, _bitmap.Height));

                    _bitmap.Dispose();
                }
            }

            Stream compoundImageStream = new MemoryStream(4000000);
            _compoundImage.Save(compoundImageStream, ImageFormat.Png);
            _compoundImage.Save($"/Users/ayylmao/Downloads/bobs/{keyedCompoundImage.CompoundImageId}bob.png");


            // If stitched image is greater than 4MB
            if (compoundImageStream.Length > 20000)
            {
                do
                {
                    compoundImageStream.Position = 0;
                    _compoundImage = ResizeImage(new Bitmap(compoundImageStream), Convert.ToInt32(_compoundImage.Width * 0.9), Convert.ToInt32(_compoundImage.Height * 0.9));
                }
                while (compoundImageStream.Length > 20000);

                return _compoundImage;
            }

            return _compoundImage;
        }

        private Bitmap CreateBitmapFromUrl(string location)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(location);
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            return new Bitmap(responseStream);
        }

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
