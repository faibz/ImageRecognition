using log4net.Config;
using Topshelf;

namespace DistributedSystems.WorkerManager
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            XmlConfigurator.Configure();

            HostFactory.Run(configurator =>
            {
                configurator.Service<WorkerManagerService>(service =>
                {
                    service.ConstructUsing(s => new WorkerManagerService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.UseLog4Net();
                configurator.SetServiceName("WorkerManagerService");
                configurator.SetDisplayName("WorkerManagerService");
                configurator.SetDescription("Service to manager the worker pool for Distributed Systems.");
            });
        }
    }
}
