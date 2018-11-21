using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystems.API.Adapters
{
    public class ServiceBusAdapter : IQueueAdapter
    {
        private readonly IQueueClient _queueClientPrimary;
        private readonly IQueueClient _queueClientSecondary;

        public ServiceBusAdapter(IConfiguration config)
        {
            _queueClientPrimary = new QueueClient(config.GetValue<string>("Azure:ServiceBusConnectionString"), config.GetValue<string>("Azure:ServiceBusQueueName"));
            _queueClientSecondary = new QueueClient(config.GetValue<string>("Azure:ServiceBusConnectionString"), config.GetValue<string>("Azure:ServiceBusQueueNameSecondary"));
        }

        public async Task<bool> SendMessage(string message)
        {
            try
            {
                await _queueClientPrimary.SendAsync(new Message(Encoding.UTF8.GetBytes(message)));
                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        public async Task<bool> SendMessage(object obj)
        {
            try
            {
                await _queueClientPrimary.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj))));
                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        public async Task<bool> SendMessageSecondary(string message)
        {
            try
            {
                await _queueClientSecondary.SendAsync(new Message(Encoding.UTF8.GetBytes(message)));
                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        public async Task<bool> SendMessageSecondary(object obj)
        {
            try
            {
                await _queueClientSecondary.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj))));
                return true;
            }
            catch (Exception)
            { }

            return false;
        }
    }
}