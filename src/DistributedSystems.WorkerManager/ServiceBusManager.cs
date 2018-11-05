using Microsoft.Azure.ServiceBus.Management;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager
{
    public class ServiceBusManager
    {
        private ManagementClient _serviceBusManager;
        private readonly List<string> _queueNames;

        public ServiceBusManager(string connectionString, string queueName)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            _queueNames = new List<string> { queueName };
        }

        public ServiceBusManager(string connectionString, List<string> queueNames)
        {
            _serviceBusManager = new ManagementClient(connectionString);
            _queueNames = queueNames;
        }

        public async Task<long> GetMessageCount()
        {
            var messageCount = 0L;

            foreach (var queue in _queueNames)
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
