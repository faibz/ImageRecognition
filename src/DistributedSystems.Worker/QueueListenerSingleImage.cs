using System;
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
    public class QueueListenerSingleImage
    {
        /* TODO:
         * - DONE create new Service Bus for compound images,
         * - update connection string and queue name in the configuration file,
         * - DONE create QueueListenerMultipleImages class -- queue multi,
         *          send stitched image to the Azure Vision;
         *          send the tags to the API;
         * - DONE rename QueueListener class to QueueListenerSingleImage -- queue single,
         * - create ImageStitcher class
         *          do stitching;
         *          decrease quality/size;
         * - DONE add another method to ImageAnalyser to send the stitched image.
         *      - (why?) The current method uses the message string, not a new stitched thing.
         * 
         * - version checker on startup
         */

        private readonly HttpClient _httpClient;
        private readonly IQueueClient _queueClient;
        private ImageAnalyser _imageAnalyser;

        public QueueListenerSingleImage(IConfigurationRoot config)
        {
            _httpClient = new HttpClient();
            _queueClient = new QueueClient(config["SingleImageServiceBusConnectionString"], config["SingleImageServiceBusQueueName"]);
            _imageAnalyser = new ImageAnalyser();
        }

        public async Task Run()
        {
            // Register the queue message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();
            await _queueClient.CloseAsync();
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageBodyString = Encoding.UTF8.GetString(message.Body);

            var imageTagData = await _imageAnalyser.ProcessSingleImage(messageBodyString);

            var sendTagsRequest = new HttpRequestMessage
            {
                RequestUri = new Uri("https://distsysimageapi.azurewebsites.net/api/Tags/SubmitImageTags"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(imageTagData), Encoding.UTF8, "application/json")
            };
            sendTagsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var sendTagsResponse = await _httpClient.SendAsync(sendTagsRequest);

            if (sendTagsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
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
