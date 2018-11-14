using System;
using System.Collections.Generic;

namespace DistributedSystems.Worker
{
    public class ImageStitcher
    {
        public ImageStitcher()
        {
            List<System.Drawing.Bitmap> imagesList = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap compoundImage = null; // Final, stitched image
        }

        System.Drawing.Bitmap StitchImages(CompoundImage images)
        {
            try
            {
                int width = 0, height = 0;

                foreach (var image in images.Images)
                {
                    //TODO: Get the images out, add them to each other accordingly with the coordinates.
                    //var ll =Image.FromFile(../ asd.bmp);
                    //var lx = Graphics.From

                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    imagesList.Add(bitmap);
                }

                compoundImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(compoundImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in imagesList)
                    {
                        g.DrawImage(image, new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return compoundImage;
            }
            catch (Exception ex)
            {
                if (compoundImage != null)
                    compoundImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in imagesList)
                {
                    image.Dispose();
                }
            }
        }
    }
}
