using Microsoft.Azure.ServiceBus.Management;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager
{
    public class ServiceBusManager
    {
        private ManagementClient _serviceBusManager;
        private readonly string _queueName;

        public ServiceBusManager(string connectionString, string queueName)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            _queueName = queueName;
        }

        public async Task<long> GetMessageCount()
        {
            var queueInfo = await _serviceBusManager.GetQueueRuntimeInfoAsync(_queueName);
            return queueInfo.MessageCountDetails.ActiveMessageCount;
        }
    }
}
