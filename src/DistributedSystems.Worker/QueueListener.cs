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
    public class QueueListener
    {
        // TODO:
        // - check response from API before closing the queue,
        // - check if the confidence is satisfactory,

        private readonly HttpClient _httpClient;
        private readonly IQueueClient _queueClient;
        private ImageAnalyser _imageAnalyser;

        public QueueListener(IConfigurationRoot config)
        {
            _httpClient = new HttpClient();
            _queueClient = new QueueClient(config["ServiceBusConnectionString"], config["ServiceBusQueueName"]);
        }

        public async Task Run()
        {
            // Register the queue message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();
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
            var bob = JsonConvert.DeserializeObject<Image>(messageBodyString);

            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{messageBodyString}");

            _imageAnalyser = new ImageAnalyser(messageBodyString);
            var analysedPicture = await _imageAnalyser.SendImageUrlToComputerVisionApiAsync();
            var lx = JObject.Parse(analysedPicture);
            var bob2 = (JArray)lx["tags"];
            var tags = bob2.ToObject<Tag[]>();

            var imageTagData = new ImageTagData
            {
                ImageId = bob.Id,
                TagData = tags,
                Key = bob.ImageKey
            };

            Console.WriteLine(analysedPicture);

            var sendTagsRequest = new HttpRequestMessage
            {
                RequestUri = new Uri("https://distsysimageapi.azurewebsites.net/api/Tags/SubmitImageTags"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(imageTagData), Encoding.UTF8, "application/json")
            };
            sendTagsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var sendTagsResponse = await _httpClient.SendAsync(sendTagsRequest);
            Console.WriteLine(sendTagsResponse.StatusCode);
            Console.WriteLine(sendTagsResponse.ReasonPhrase);

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
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
