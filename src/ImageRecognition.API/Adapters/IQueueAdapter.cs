using System.Threading.Tasks;

namespace ImageRecognition.API.Adapters
{
    public interface IQueueAdapter
    {
        Task<bool> SendMessage(object obj, string label = null);
    }
}