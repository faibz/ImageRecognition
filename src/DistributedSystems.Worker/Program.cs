using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DistributedSystems.Worker
{
    class Program
    {
        public static async Task Main(string[] args)
        //public static void Main(string[] args)
        {
            // Apply the appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            //var queueListenerSingleImage = new QueueListenerSingleImage(configuration);
            //var queueListenerCompoundImage = new QueueListenerCompoundImage(configuration);

            //var singleImageListener = queueListenerSingleImage.Run();
            //var compoundImageListener = queueListenerCompoundImage.Run();
            //Task.WaitAll(singleImageListener, compoundImageListener);
            //Task.WaitAll(singleImageListener, compoundImageListener).Wait(10000);

            var queueListener = new QueueListener(configuration);
            await queueListener.Run();
        }
    }
}

/* TODO:
 * - DONE update connection string and queue name in the configuration file;
 * - DONE double check the API endpoint in QueueListenerCompoundImage (appsettings.json);
 * - increase MaxConcurrentCalls in both queue listeners and see if the Worker blows up;
 * - DONE? decrease quality/size if the size exceeds 4MB;
 * - version checker on startup;
 * 
 * 
 * - remove commented out code snippets from Program.cs;
 * 
 * 
 * - TEST the whole thing;
 * 
 * 
 * - remove TODO from ImageStitcher: line 55;
 * - remove debug comments from QueueListener;
 * - delete old QueueListener's;
 * - remove this TODO;
 */
