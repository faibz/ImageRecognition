using log4net.Core;
using System;
using System.ServiceProcess;
using System.Timers;

namespace DistributedSystems.WorkerManager
{
    public partial class WorkerManagerService : ServiceBase
    {
        private ILogger _logger;
        private Timer _timer = new Timer();

        public WorkerManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger.Log(typeof(WorkerManagerService), Level.Info, $"Service starting. Time (UTC): {DateTime.UtcNow}", null);

            _timer.Elapsed += new ElapsedEventHandler(DoAThing);
            _timer.Interval = 5000;
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _logger.Log(typeof(WorkerManagerService), Level.Info, $"Service stopping. Time (UTC): {DateTime.UtcNow}", null);
        }

        private void DoAThing(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
