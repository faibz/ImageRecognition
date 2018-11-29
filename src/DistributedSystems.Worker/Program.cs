using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DistributedSystems.Worker
{
    class Program
    {
        //public static async Task Main(string[] args)
        public static void Main(string[] args)
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
            Task.WaitAll(singleImageListener, compoundImageListener);
            //Task.WaitAll(singleImageListener, compoundImageListener).Wait(10000);
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
 * - DONE? do stitching;
 *      - DONE add conditions to stitching to not only stitch to the right;
 * - DONE? send stitched image to the Azure Vision;
 *      - DONE? REWRITE ImageAnalyser FOR CompoundImage's TO USE THE SDK
 *          - https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-analyze-sdk
 * - DONE? send the tags to the API;
 * 
 * 
 * - remove TODO from AzureVision: line 59;
 * - remove old Api call from AzureVision class (commented out at the bottom of the file);
 * - remove commented out code snippets from Program.cs;
 * 
 * 
 * - TEST the whole thing;
 * 
 * 
 * - remove TODO from ImageStitcher: line 55;
 * - remove this TODO;
 */
