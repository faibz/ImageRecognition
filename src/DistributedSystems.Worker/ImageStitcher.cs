using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DistributedSystems.API.Models;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        public Bitmap StitchImages(KeyedCompoundImage keyedCompoundImage)
        {
            List<int> _coordsX = new List<int>();
            List<int> _coordsY = new List<int>();
            Bitmap _compoundImage = null; // Final, stitched image

            // Minimum width and height assumming that every image has equal size.
            Bitmap _anyImage = new Bitmap(keyedCompoundImage.Images[0].Image.Location);
            int _width = _anyImage.Width;
            int _height = _anyImage.Height;
            _anyImage.Dispose();

            // Store coordinates of each image.
            foreach (var image in keyedCompoundImage.Images)
            {
                _coordsX.Add(image.Coordinate.X);
                _coordsY.Add(image.Coordinate.Y);
            }

            _coordsX.OrderBy(coord => coord);
            _coordsY.OrderBy(coord => coord);

            // Calculate canvas dimensions using the images count on each axis.
            _width  *= _coordsX[_coordsX.Count - 1] - _coordsX[0];
            _height *= _coordsY[_coordsY.Count - 1] - _coordsY[0];
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
                    _bitmap = new Bitmap(image.Image.Location);
                    _offsetX = _bitmap.Width * image.Coordinate.X - _bitmap.Width;
                    _offsetY = _height - (_bitmap.Height * image.Coordinate.Y - _bitmap.Height) - _bitmap.Height;

                    g.DrawImage(_bitmap, new Rectangle(_offsetX, _offsetY, _bitmap.Width, _bitmap.Height));

                    _bitmap.Dispose();
                }
            }

            return _compoundImage;
        }
    }
}
