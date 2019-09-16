using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ImageRecognition.API.Adapters
{
    public class ServiceBusAdapter : IQueueAdapter
    {
        private readonly IQueueClient _queueClientPrimary;

        public ServiceBusAdapter(IConfiguration config)
        {
            _queueClientPrimary = new QueueClient(config.GetValue<string>("Azure:ServiceBusConnectionString"), config.GetValue<string>("Azure:ServiceBusQueueName"));
        }

        public async Task<bool> SendMessage(object obj, string label = "single-image")
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj))) { Label = label };

                await _queueClientPrimary.SendAsync(message);
                return true;
            }
            catch (Exception)
            { }

            return false;
        }
    }
}