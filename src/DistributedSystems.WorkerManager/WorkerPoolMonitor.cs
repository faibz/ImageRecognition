using System.Collections.Generic;

namespace DistributedSystems.WorkerManager
{
    internal class WorkerPoolMonitor
    {
        public IList<Worker> TotalWorkers { get; private set; }
        //https://docs.chef.io/azure_portal.html
    }

    public class Worker
    {
        public bool Active { get; private set; }

        public void TurnOn()
        {
            if (Active) return;

            Active = true;

            /*
             * do turn on stuff here 
             */
        }

        public void ShutDown()
        {
            if (!Active) return;

            Active = false;

            /*
             * do shut down stuff here
             */
        }
    }
}