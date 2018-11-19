using log4net;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
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
            _workerPoolMonitor = new WorkerPoolMonitor(System.Configuration.ConfigurationManager.AppSettings["AzureAuthFileLocation"]);
        }

        protected override void OnStart(string[] args)
        {
            _log.Info($"Service starting at {DateTime.UtcNow}.");

            if (_serviceHost != null) _serviceHost.Close();

            _serviceHost = new ServiceHost(typeof(WorkerManagerService));
            _serviceHost.AddServiceEndpoint(typeof(IManager), new BasicHttpBinding(), _baseAddress); // do I need this? does it replace config
            _serviceHost.Open();

            _timer.Elapsed += new ElapsedEventHandler(ManageWorkerPool);
            _timer.Interval = 5000;
            _timer.Enabled = true;

            _log.Info($"Service started successfully at {DateTime.UtcNow}.");
        }

        protected override void OnStop()
        {
            _log.Info($"Service stopping. Time (UTC): {DateTime.UtcNow}.");

            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }

            _log.Info($"Service stopped successfully at {DateTime.UtcNow}.");
        }

        private async void ManageWorkerPool(object sender, ElapsedEventArgs e)
        {
            var queueMessageCount = await _serviceBusManager.GetMessageCount();
            var activeWorkerCount = CurrentWorkerCount();

            _log.Info($"Checking service bus and workers. Time (UTC): {DateTime.UtcNow}. Amount of message in queues: {queueMessageCount}. Amount of active workers: {activeWorkerCount}.");

            switch(WorkerQueueEvaluator.AdviseAction(activeWorkerCount, queueMessageCount))
            {
                case WorkerAction.Add:
                    _log.Info($"Starting new worker at: {DateTime.UtcNow}.");
                    await StartNewWorker();
                    break;
                case WorkerAction.Remove:
                    _log.Info($"Shutting worker down at: {DateTime.UtcNow}.");
                    await ShutWorkerDown();
                    break;
                default:
                    break;
            }
        }

        private async Task StartNewWorker() 
            => await _workerPoolMonitor.Workers.Where(worker => !worker.PowerState).First().TurnOn();

        private async Task ShutWorkerDown()
            => await _workerPoolMonitor.Workers.Where(worker => worker.PowerState).First().TurnOff();

        public int TotalWorkerCount()
            => _workerPoolMonitor.Workers.Count;

        public int CurrentWorkerCount() 
            => _workerPoolMonitor.Workers.Where(worker => worker.PowerState).Count();
    }
}
