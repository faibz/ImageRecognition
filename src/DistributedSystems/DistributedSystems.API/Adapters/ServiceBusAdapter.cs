using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{
    public class ServiceBusAdapter : IQueueAdapter
    {
        private readonly IQueueClient _queueClient;

        public ServiceBusAdapter(IConfiguration config)
        {
            _queueClient = new QueueClient(config.GetValue<string>("Azure:ServiceBusConnectionString"), config.GetValue<string>("Azure:ServiceBusQueueName"));
        }

        public async Task SendMessage(string message) 
            => await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes("messageToAddToQueue")));

        public async Task SendMessage(object obj) 
            => await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj))));
    }
}