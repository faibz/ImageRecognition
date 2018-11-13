using DistributedSystems.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{

    public class AzureBlobStorageAdapter : IFileStorageAdapter
    {
        private readonly CloudBlobContainer _blobContainer;

        public AzureBlobStorageAdapter(IConfiguration config)
        {
            _blobContainer = CloudStorageAccount.Parse(config.GetValue<string>("Azure:CloudStorageConnectionString"))
                .CreateCloudBlobClient()
                .GetContainerReference(config.GetValue<string>("Azure:CloudBlobContainerName"));
        }

        public async Task<string> UploadImage(Guid imageId, MemoryStream memoryStream)
        {
            try
            {
                await _blobContainer.CreateIfNotExistsAsync();

                var blockBlob = _blobContainer.GetBlockBlobReference($"{imageId.ToString()}.jpg");
                await blockBlob.UploadFromStreamAsync(memoryStream);

                return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
            }
            catch (Exception)
            { }

            return null;
        }

        public async Task<string> GetImageUriWithKey(Guid imageId)
        {
            try
            {
                if (!await _blobContainer.ExistsAsync()) return string.Empty;

                var blockBlob = _blobContainer.GetBlockBlobReference($"{imageId.ToString()}.jpg");
                return blockBlob.StorageUri.PrimaryUri.AbsoluteUri + GenerateSharedAccessSignature(blockBlob);
            }
            catch (Exception)
            { }

            return null;

        }

        public string GenerateSharedAccessSignature(CloudBlockBlob blockBlob)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-1),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            return blockBlob.GetSharedAccessSignature(sasPolicy);
        }
    }
}