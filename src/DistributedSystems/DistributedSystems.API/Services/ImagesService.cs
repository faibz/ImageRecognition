using System.IO;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Results;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Services
{
    public interface IImagesService
    {
        Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream);
    }

    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;

        public ImagesService(IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }


        public async Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream)
        {
            //TODO: EITHER UPLOAD TO BLOB STORAGE IN AZURE OR TO OWN DISTRIBUTED FILE STORAGE SYSTEM
            //blob.UploadFile(memoryStream);
            await _imagesRepository.InsertImage(image);
            //serviceBus.AddImageProcessMessage(image, memoryStream);

            return new UploadImageResult(true, image);
        }
    }
}