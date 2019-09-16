using log4net.Config;
using Topshelf;

namespace ImageRecognition.WorkerManager
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
                configurator.SetDisplayName("Worker Manager Service (Distributed Systems)");
                configurator.SetDescription("Service to manage the worker VM pool. (Distributed Systems)");
            });
        }
    }
}
