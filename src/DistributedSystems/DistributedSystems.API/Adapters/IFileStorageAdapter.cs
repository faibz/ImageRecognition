using System;
using System.IO;
using System.Threading.Tasks;
using DistributedSystems.API.Models;

namespace DistributedSystems.API.Adapters
{
    public interface IFileStorageAdapter
    {
        Task<string> UploadImage(Image image, MemoryStream memoryStream);
        Task<string> GetImageUriWithKey(Guid imageId);
    }
}