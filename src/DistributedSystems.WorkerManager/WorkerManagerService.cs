using log4net;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;

namespace DistributedSystems.WorkerManager
{
    public partial class WorkerManagerService : ServiceBase, IManager
    {
        private ServiceHost _serviceHost = null;
        private string _baseAddress = "http://localhost/WorkerManagerService";

        private ILog _log;
        private Timer _timer = new Timer();

        private readonly ServiceBusManager _serviceBusManager;
        private readonly WorkerPoolMonitor _workerPoolMonitor;

        public WorkerManagerService()
        {
            InitializeComponent();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();

            _serviceBusManager = new ServiceBusManager(System.Configuration.ConfigurationManager.AppSettings["ServiceBusConnectionString"], System.Configuration.ConfigurationManager.AppSettings["ServiceBusQueueName"]);
            _workerPoolMonitor = new WorkerPoolMonitor();
        }

        protected override void OnStart(string[] args)
        {
            _log.Info($"Service starting. Time (UTC): {DateTime.UtcNow}.");

            if (_serviceHost != null) _serviceHost.Close();

            _serviceHost = new ServiceHost(typeof(WorkerManagerService));
            _serviceHost.AddServiceEndpoint(typeof(IManager), new BasicHttpBinding(), _baseAddress); // do I need this? does it replace config
            _serviceHost.Open();

            _timer.Elapsed += new ElapsedEventHandler(DoAThing);
            _timer.Interval = 5000;
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _log.Info($"Service stopping. Time (UTC): {DateTime.UtcNow}.");

            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }

        private async void DoAThing(object sender, ElapsedEventArgs e)
        {
            _log.Info($"Checking service bus. Time (UTC): {DateTime.UtcNow}.");

            var queueMessageCount = await _serviceBusManager.GetMessageCount();
            var activeWorkerCount = CurrentWorkerCount();

            if (queueMessageCount > 1L && queueMessageCount < 10L)
            {
                _log.Info("Normal level");
            }

            if (queueMessageCount >= 10L)
            {
                _log.Warn("Something or other");

                StartNewWorker();
            }
        }

        private void StartNewWorker() 
            => _workerPoolMonitor.TotalWorkers.Where(worker => !worker.Active).First().TurnOn();

        private void ShutWorkerDown()
            => _workerPoolMonitor.TotalWorkers.Where(worker => worker.Active).First().ShutDown();

        public int TotalWorkerCount()
            => _workerPoolMonitor.TotalWorkers.Count;

        public int CurrentWorkerCount() 
            => _workerPoolMonitor.TotalWorkers.Where(worker => worker.Active).Count();
    }
}
