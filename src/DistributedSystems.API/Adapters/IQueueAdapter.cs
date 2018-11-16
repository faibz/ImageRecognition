using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{
    public interface IQueueAdapter
    {
        Task<bool> SendMessage(string message);
        Task<bool> SendMessage(object obj);
        Task<bool> SendMessageSecondary(string message);
        Task<bool> SendMessageSecondary(object obj);
    }
}