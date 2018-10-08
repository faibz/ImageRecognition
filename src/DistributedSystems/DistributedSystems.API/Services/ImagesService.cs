using System.IO;
using System.Text;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Models.Results;
using DistributedSystems.API.Repositories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DistributedSystems.API.Services
{
    public interface IImagesService
    {
        Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream);
    }

    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;

        private readonly IQueueClient _queueClient;
        private readonly CloudBlobContainer _blobContainer;

        public ImagesService(IConfiguration config, IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
            _queueClient = new QueueClient(config.GetValue<string>("ServiceBusConnectionString"), config.GetValue<string>("ServiceBusQueueName"));
            _blobContainer = CloudStorageAccount.Parse("CloudStorageConnectionString")
                .CreateCloudBlobClient()
                .GetContainerReference(config.GetValue<string>("blobName"));
        }


        public async Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream)
        {
            //TODO MEMES
            //await _blobContainer.CreateIfNotExistsAsync();
            //Decide on blockBlobReference. Id from DB?

            await _blobContainer.GetBlockBlobReference("blockBlobRef").UploadFromStreamAsync(memoryStream); //can still change to diff file system
            await _imagesRepository.InsertImage(image);
            await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes("messageToAddToQueue")));

            return new UploadImageResult(true, image);
        }
    }
}