using System.Collections.Generic;
using System.IO;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Requests;

namespace DistributedSystems.API.Validators
{
    public interface IImageValidator
    {
        IList<Error> ValidateImageRequest(ImageRequest imageRequest);
    }

    public class ImageValidator : IImageValidator
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