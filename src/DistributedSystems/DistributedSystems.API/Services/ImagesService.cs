using System;
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
            _queueClient = new QueueClient(config.GetValue<string>("Azure:ServiceBusConnectionString"), config.GetValue<string>("Azure:ServiceBusQueueName"));
            _blobContainer = CloudStorageAccount.Parse("Azure:CloudStorageConnectionString")
                .CreateCloudBlobClient()
                .GetContainerReference(config.GetValue<string>("Azure:CloudBlobContainerName"));
        }


        public async Task<UploadImageResult> UploadImage(Image image, MemoryStream memoryStream)
        {
            //TODO MEMES
            //await _blobContainer.CreateIfNotExistsAsync();
            //Decide on blockBlobReference. Id from DB?
            //Does the SAS helper require settings to be configured in azure?
            //set stuff up to allow failures to be shown


            var blockBlob = _blobContainer.GetBlockBlobReference("blockBlobRef");
            await blockBlob.UploadFromStreamAsync(memoryStream); //can still change to diff file system

            //TODO: MOVE THIS INTO SAS HELPER
            var lx2 = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-1),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(5)
            };

            blockBlob.GetSharedAccessSignature(lx2);
            image.Location = blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
            await _imagesRepository.InsertImage(image);
            await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes("messageToAddToQueue")));


            return new UploadImageResult(true, image);
        }
    }
}