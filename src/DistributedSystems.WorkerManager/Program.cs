using System.ServiceProcess;

namespace DistributedSystems.WorkerManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WorkerManagerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
