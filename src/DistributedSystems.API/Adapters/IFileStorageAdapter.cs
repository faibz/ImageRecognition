using System;
using System.IO;
using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{
    public interface IFileStorageAdapter
    {
        Task<string> UploadImage(Guid imageId, MemoryStream memoryStream);
        Task<string> GetImageUriWithKey(Guid imageId);
    }
}