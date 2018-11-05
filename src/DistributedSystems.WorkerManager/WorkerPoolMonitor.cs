using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Collections.Generic;

namespace DistributedSystems.WorkerManager
{
    internal class WorkerPoolMonitor
    {
        public WorkerPoolMonitor(string azureAuthFileLocation)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromFile(azureAuthFileLocation);
            var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();

            //TODO: store all vm names? get all vm names?
            foreach (var item in new List<string>())
            {
                Workers.Add(new Worker(azure.VirtualMachines.GetByResourceGroup("distributed-systems", item)));
            }
        }

        public IList<Worker> Workers { get; private set; } = new List<Worker>();
        //https://docs.chef.io/azure_portal.html
    }
}