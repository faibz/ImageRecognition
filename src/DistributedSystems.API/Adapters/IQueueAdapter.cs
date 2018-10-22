using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{
    public interface IQueueAdapter
    {
        Task SendMessage(string message);
        Task SendMessage(object obj);
    }
}