using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace DistributedSystems.WorkerManager.WorkerManager
{
    public class ServiceBusManager
    {
        private readonly ManagementClient _serviceBusManager;
        public List<string> QueueNames { get; }

        public ServiceBusManager(string connectionString, string queueName)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            QueueNames = new List<string> { queueName };
        }

        public ServiceBusManager(string connectionString, List<string> queueNames)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            QueueNames = queueNames;
        }

        public async Task<long> GetMessageCount()
        {
            var messageCount = 0L;

            foreach (var queue in QueueNames)
            {
                try
                {
                    messageCount += (await _serviceBusManager.GetQueueRuntimeInfoAsync(queue)).MessageCountDetails.ActiveMessageCount;
                }
                catch (System.Exception)
                { }
            }

            return messageCount;
        }
    }
}
