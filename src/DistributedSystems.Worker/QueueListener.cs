using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistributedSystems.Shared.Models.Requests;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DistributedSystems.Worker
{
    public class QueueListener
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationRoot _config;
        private readonly IQueueClient _queueClient;
        private AzureVision _imageAnalyser;

        public QueueListener(IConfigurationRoot config)
        {
            _httpClient = new HttpClient();
            _config = config;
            _queueClient = new QueueClient(_config["ServiceBusConnectionString"], _config["ServiceBusQueueName"]);
            _imageAnalyser = new AzureVision(_config);
        }

        public async Task Run()
        {
            // Register the queue message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.WriteLine("1: finished RegisterOnMessageHandlerAndReceiveMessages");

            Console.ReadKey();

            await _queueClient.CloseAsync();
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Register the function that processes messages.
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            Console.WriteLine("2: finished ProcessMessagesAsync");
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            HttpResponseMessage sendTagsResponse;
            var messageBodyString = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine("5: got into ProcessMessagesAsync");

            if (message.Label == "single-image")
            {
                var tagData = await _imageAnalyser.ProcessSingleImage(messageBodyString);
                Console.WriteLine("3: finished ProcessSingleImage");

                if (tagData.GetType() == typeof(MapTagData))
                {
                    var tagDataString = JsonConvert.SerializeObject(tagData);
                    sendTagsResponse = await SendTags(_config["ApiSubmitMapImagePartTags"], tagDataString);
                    Console.WriteLine("4: sent stuff to the API");
                }
                else
                {
                    var tagDataString = JsonConvert.SerializeObject(tagData);
                    sendTagsResponse = await SendTags(_config["ApiSubmitImageTags"], tagDataString);
                    Console.WriteLine("4: sent stuff to the API");
                }

                if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            else if (message.Label == "compound-image")
            {
                var tagData = JsonConvert.SerializeObject(await _imageAnalyser.ProcessCompoundImage(messageBodyString));
                Console.WriteLine("3: finished ProcessCompoundImage");

                sendTagsResponse = await SendTags(_config["ApiSubmitMapCompoundImageTags"], tagData);
                Console.WriteLine("4: sent stuff to the API");

                if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            else
            {
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }

        private async Task<HttpResponseMessage> SendTags(string endpoint, string tagData)
        {
            var sendTagsRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = HttpMethod.Post,
                Content = new StringContent(tagData, Encoding.UTF8, "application/json")
            };
            sendTagsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await _httpClient.SendAsync(sendTagsRequest);
        }

        // Use this handler to examine the exceptions received on the message pump.
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
