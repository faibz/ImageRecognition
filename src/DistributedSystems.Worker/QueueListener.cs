using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            // Register the queue message handler and receive messages in a loop...
            RegisterOnMessageHandlerAndReceiveMessages();

            // ...until the user presses a key.
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
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            HttpResponseMessage sendTagsResponse;
            var messageBodyString = Encoding.UTF8.GetString(message.Body);

            if (message.Label == "single-image")
            {
                var tagData = JsonConvert.SerializeObject(await _imageAnalyser.ProcessSingleImage(messageBodyString));
                Console.WriteLine("Processed a SINGLE IMAGE.");

                sendTagsResponse = await SendTags(_config["ApiSubmitImageTags"], tagData);
                Console.WriteLine("Submitted SINGLE IMAGE tags to the API.");

                // If the API responds with '200', close the message.
                if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            else if (message.Label == "compound-image")
            {
                var tagData = JsonConvert.SerializeObject(await _imageAnalyser.ProcessCompoundImage(messageBodyString));
                Console.WriteLine("Processed a COMPOUND IMAGE.");

                sendTagsResponse = await SendTags(_config["ApiSubmitMapCompoundImageTags"], tagData);
                Console.WriteLine("Submitted COMPOUND IMAGE tags to the API.");

                // If the API responds with '200', close the message.
                if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            else
            {
                // Default: If there is no matching 'Label' key on the message, close it.
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
            Console.WriteLine($"Message handler encountered an exception:\n{exceptionReceivedEventArgs.Exception}.\n\n");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint:\n{context.Endpoint}\n");
            Console.WriteLine($"- Entity Path:\n{context.EntityPath}\n");
            Console.WriteLine($"- Executing Action:\n{context.Action}\n\n\n\n");
            return Task.CompletedTask;
        }
    }
}
