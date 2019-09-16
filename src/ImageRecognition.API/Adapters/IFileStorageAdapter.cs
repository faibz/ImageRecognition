using System.IO;
using System.Threading.Tasks;

namespace ImageRecognition.API.Adapters
{
    public interface IFileStorageAdapter
    {
        Task<string> UploadFile(string fileName, MemoryStream memoryStream, string containerName = null);
        Task<string> GetFileUriWithKey(string fileName, string containerName = null);
    }
}