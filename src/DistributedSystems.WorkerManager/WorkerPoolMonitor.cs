using System.Collections.Generic;

namespace DistributedSystems.WorkerManager
{
    internal class WorkerPoolMonitor
    {
        public IList<Worker> ActiveWorkers { get; private set; }
        //https://docs.chef.io/azure_portal.html
    }

    public class Worker
    {
    }
}