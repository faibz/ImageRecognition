using System;
using System.Collections.Generic;
using System.Drawing;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        List<Bitmap> _imagesList;
        Bitmap _compoundImage; // Final, stitched image

        public ImageStitcher()
        {
            _imagesList = new List<Bitmap>();
            _compoundImage = null;
        }

        public Bitmap StitchImages(CompoundImage images)
        {
            try
            {
                int width = 0, height = 0;

                foreach (var image in images.Images)
                {
                    Bitmap bitmap = new Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    _imagesList.Add(bitmap);
                }

                _compoundImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(_compoundImage))
                {
                    //set background color
                    g.Clear(Color.Transparent);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (Bitmap image in _imagesList)
                    {
                        g.DrawImage(image, new Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return _compoundImage;
            }
            catch (Exception ex)
            {
                if (_compoundImage != null)
                    _compoundImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in _imagesList)
                {
                    image.Dispose();
                }
            }
        }
    }
}
