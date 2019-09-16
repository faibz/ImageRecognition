using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using ImageRecognition.WorkerManager.WorkerManager;
using log4net;

namespace ImageRecognition.WorkerManager
{
    public class WorkerManagerService
    {
        private readonly int _targetProcessingSla;
        private int _minimumWorkerCount;

        private readonly ILog _log;
        private readonly Timer _timer = new Timer();
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly ServiceBusManager _serviceBusManager;
        private readonly WorkerPoolMonitor _workerPoolMonitor;

        public WorkerManagerService()
        {
            _log = LogManager.GetLogger(typeof(WorkerManagerService));
            _targetProcessingSla = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetProcessingSla"]);
            _minimumWorkerCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MinimumWorkerCount"]);
            _httpClient.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ImageAPIBaseAddress"]);
            _serviceBusManager = new ServiceBusManager(System.Configuration.ConfigurationManager.AppSettings["ServiceBusConnectionString"], System.Configuration.ConfigurationManager.AppSettings["ServiceBusQueueName"]);
            _workerPoolMonitor = new WorkerPoolMonitor(System.Configuration.ConfigurationManager.AppSettings["AzureAuthFileLocation"]);
            if (_minimumWorkerCount > TotalWorkerCount()) _minimumWorkerCount = TotalWorkerCount();
        }

        public void Start()
        {
            _timer.Elapsed += ManageWorkerPool;
            _timer.Interval = 30000;
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _httpClient.Dispose();
        }

        private async void ManageWorkerPool(object sender, ElapsedEventArgs e)
        {
            var queueMessageCount = await _serviceBusManager.GetMessageCount();
            var activeWorkerCount = CurrentWorkerCount();
            var totalWorkerCount = TotalWorkerCount();

            _log.Info($"Checking service bus and workers. Amount of message in queues: {queueMessageCount}. Amount of workers (active/total): {activeWorkerCount}/{totalWorkerCount}.");

            switch (await WorkerQueueEvaluator.AdviseAction(activeWorkerCount, totalWorkerCount, queueMessageCount, _targetProcessingSla, _minimumWorkerCount, _httpClient))
            {
                case WorkerAction.Add:
                    _log.Info("Starting new worker.");
                    await StartNewWorker();
                    break;
                case WorkerAction.Remove:
                    _log.Info("Shutting worker down.");
                    await ShutWorkerDown();
                    break;
                default:
                    _log.Info("No worker action taken.");
                    break;
            }
			
			await _workerPoolMonitor.UpdateAllWorkers();
        }

        private async Task StartNewWorker()
        {
            if (_workerPoolMonitor.Workers.Any(worker => !worker.PowerState)) await _workerPoolMonitor.Workers.First(worker => !worker.PowerState).TurnOn();
        }

        private async Task ShutWorkerDown()
        {
            if (_workerPoolMonitor.Workers.Any(worker => worker.PowerState)) await _workerPoolMonitor.Workers.First(worker => worker.PowerState).TurnOff();
        }

        public int TotalWorkerCount()
            => _workerPoolMonitor.Workers.Count;

        public int CurrentWorkerCount()
            => _workerPoolMonitor.Workers.Count(worker => worker.PowerState);
    }
}
