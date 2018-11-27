using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Collections.Generic;

namespace DistributedSystems.WorkerManager.WorkerManager
{
    internal class WorkerPoolMonitor
    {
        public WorkerPoolMonitor(string azureAuthFileLocation)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromFile(azureAuthFileLocation);
            var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();

            var vms = azure.VirtualMachines.ListByResourceGroup("distributed-systems");
            
            foreach (var vm in vms)
            {
                Workers.Add(new Worker(vm));
            }
        }

        public IList<IWorker> Workers { get; } = new List<IWorker>();
    }
}