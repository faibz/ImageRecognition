using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{

    public class AzureBlobStorageAdapter : IFileStorageAdapter
    {
        private readonly CloudBlobClient _blobClient;

        public AzureBlobStorageAdapter(string connectionString, string containerName)
        {
            _blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
        }

        public async Task<string> UploadFile(string fileName, MemoryStream memoryStream, string containerName = null)
        {
            if (string.IsNullOrEmpty(containerName)) return null;

            try
            {
                var blobContainer = _blobClient.GetContainerReference(containerName);
                await blobContainer.CreateIfNotExistsAsync();

                var blockBlob = blobContainer.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromStreamAsync(memoryStream);

                return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
            }
            catch (Exception)
            { }

            return null;
        }

        public async Task<string> GetFileUriWithKey(string fileName, string containerName = null)
        {
            if (string.IsNullOrEmpty(containerName)) return null;

            try
            {
                var blobContainer = _blobClient.GetContainerReference(containerName);
                if (!await blobContainer.ExistsAsync()) return string.Empty;

                var blockBlob = blobContainer.GetBlockBlobReference(fileName);
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