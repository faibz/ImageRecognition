using System.Collections.Generic;
using System.IO;
using ImageRecognition.Shared.Models;
using ImageRecognition.Shared.Models.Requests;

namespace ImageRecognition.API.Validators
{
    public interface IImagesValidator
    {
        IList<Error> ValidateImageRequest(ImageRequest imageRequest);
    }

    public class ImagesValidator : IImagesValidator
    {
        private const int MaxFileLength = 4000000;

        public IList<Error> ValidateImageRequest(ImageRequest imageRequest)
        {
            var errors = new List<Error>();

            var memStream = new MemoryStream(imageRequest.Image);
            if (memStream.Length > MaxFileLength) errors.Add(new Error("image_size", "Image length must be less than than 4,000,000 bytes."));

            return errors;
        }
    }
}