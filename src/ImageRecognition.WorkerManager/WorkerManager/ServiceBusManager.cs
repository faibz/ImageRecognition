using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace ImageRecognition.WorkerManager.WorkerManager
{
    public class ServiceBusManager
    {
        private readonly ManagementClient _serviceBusManager;
        public string QueueName { get; }

        public ServiceBusManager(string connectionString, string queueName)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            QueueName = queueName;
        }

        public async Task<long> GetMessageCount() 
            => (await _serviceBusManager.GetQueueRuntimeInfoAsync(QueueName)).MessageCountDetails.ActiveMessageCount;
    }
}
