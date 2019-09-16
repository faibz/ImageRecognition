using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRecognition.WorkerManager.WorkerManager
{
    internal class WorkerPoolMonitor
    {
        public WorkerPoolMonitor(string azureAuthFileLocation)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromFile(azureAuthFileLocation);
            var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();

            var vms = azure.VirtualMachines.ListByResourceGroup("distributed-systems").Where(vm => vm.Name.Contains("worker"));

            foreach (var vm in vms)
            {
                Workers.Add(new Worker(vm));
            }
        }

        public async Task UpdateAllWorkers()
        {
            foreach (var worker in Workers)
            {
                await worker.Update();
            }
        }

        public IList<IWorker> Workers { get; } = new List<IWorker>();
    }
}