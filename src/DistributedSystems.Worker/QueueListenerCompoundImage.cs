using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistributedSystems.Worker
{
    public class QueueListenerCompoundImage
    {
        private readonly HttpClient _httpClient;
        private readonly IQueueClient _queueClient;
        private ImageAnalyser _imageAnalyser;

        public QueueListenerCompoundImage()
        {
            _httpClient = new HttpClient();
            _queueClient = new QueueClient(config["CompoundImageServiceBusConnectionString"], config["CompoundImageServiceBusQueueName"]);
            _imageAnalyser = new ImageAnalyser();
        }

        public async Task Run()
        {
            RegisterOnMessageHandlerAndReceiveMessages();
            await _queueClient.CloseAsync();
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageBodyString = Encoding.UTF8.GetString(message.Body);
            
            var mapTagData = await _imageAnalyser.ProcessCompoundImage(messageBodyString);

            var sendTagsRequest = new HttpRequestMessage
            {
                RequestUri = new Uri("https://distsysimageapi.azurewebsites.net/api/Tags/SubmitImageTags"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(mapTagData), Encoding.UTF8, "application/json")
            };
            sendTagsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var sendTagsResponse = await _httpClient.SendAsync(sendTagsRequest);
            //Console.WriteLine(sendTagsResponse.StatusCode); // DEBUG

            if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine();
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine();
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
