using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        public Bitmap StitchImages(KeyedCompoundImage keyedCompoundImage)
        {
            /* TODO:
             *      - Adjust coordinates to start from 1 on the g object.
             *      - Add changes from ImageTest.
             */
            try
            {
                List<int> _coordsX = new List<int>();
                List<int> _coordsY = new List<int>();
                Bitmap _compoundImage = null; // Final, stitched image

                // Minimum width and height assumming that every image has equal size.
                Bitmap _anyImage = new Bitmap(keyedCompoundImage.Images[0].Location);
                int _width = _anyImage.Width;
                int _height = _anyImage.Height;

                // Store coordinates of each image.
                foreach (var image in keyedCompoundImage.Images)
                {
                    _coordsX.Add(image.CoordinateX);
                    _coordsY.Add(image.CoordinateY);
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
                    var _offsetX;
                    var _offsetY;

                    // Go through each image and draw it on the _compoundImage.
                    foreach (var image in keyedCompoundImage.Images)
                    {
                        _bitmap = new Bitmap(image.Location);
                        _offsetX = _bitmap.Width * image.CoordinateX - _bitmap.Width;
                        _offsetY = _bitmap.Height * image.CoordinateY - _bitmap.Height;

                        g.DrawImage(_bitmap, new Rectangle(_offsetX, _offsetY, _bitmap.Width, _bitmap.Height));
                    }
                }

                return _compoundImage;
            }
            catch (Exception ex)
            {
                if (_compoundImage != null)
                {
                    _anyImage.Dispose();
                    _bitmap.Dispose();
                    _compoundImage.Dispose();
                }
                throw ex;
            }
            finally
            {
                _anyImage.Dispose();
                _bitmap.Dispose();
                _compoundImage.Dispose();
            }
        }
    }
}
