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

            var queueListener = new QueueListener(configuration);
            await queueListener.Run();
        }
    }
}

/* TODO:
 * - DONE update connection string and queue name in the configuration file;
 * - DONE double check the API endpoint in QueueListenerCompoundImage (appsettings.json);
 * - DONE? decrease quality/size if the size exceeds 4MB;
 * 
 * - increase MaxConcurrentCalls in both queue listeners and see if the Worker blows up;
 * - version checker on startup;
 * - TEST the whole thing;
 * - remove debug comments from QueueListener;
 * - remove this TODO;
 */
