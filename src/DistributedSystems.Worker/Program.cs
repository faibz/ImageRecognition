using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DistributedSystems.Worker
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Apply the appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var queueListenerSingleImage = new QueueListenerSingleImage(configuration);
            var queueListenerCompoundImage = new QueueListenerCompoundImage(configuration);

            var singleImageListener = queueListenerSingleImage.Run();
            var compoundImageListener = queueListenerCompoundImage.Run();
            Task.WaitAll(new[] { singleImageListener, compoundImageListener });
        }
    }
}
