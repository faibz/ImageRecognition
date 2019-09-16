using Microsoft.Azure.Management.Compute.Fluent;
using System.Threading.Tasks;

namespace ImageRecognition.WorkerManager.WorkerManager
{
    public interface IWorker
    {
        Task TurnOn();
        Task TurnOff();
        bool PowerState { get; }
        Task Update();
    }

    public class Worker : IWorker
    {
        private readonly IVirtualMachine _virtualMachine;
        private bool _powerStateUpdating;

        public Worker(IVirtualMachine virtualMachine)
        {
            _virtualMachine = virtualMachine;
        }

        public bool PowerState => _powerStateUpdating 
            ? !(_virtualMachine.PowerState.Value == "PowerState/starting" || _virtualMachine.PowerState.Value == "PowerState/running")
            : _virtualMachine.PowerState.Value == "PowerState/starting" || _virtualMachine.PowerState.Value == "PowerState/running";

        public async Task TurnOn()
        {
            if (PowerState) return;

            _powerStateUpdating = true;
            
            await _virtualMachine.StartAsync();

            var refreshTask = Task.Run(() => _virtualMachine.RefreshInstanceViewAsync());
            await refreshTask.ContinueWith(_ => _powerStateUpdating = false);
        }

        public async Task TurnOff()
        {
            if (!PowerState) return;

            _powerStateUpdating = true;
            
            await _virtualMachine.PowerOffAsync();

            var refreshTask = Task.Run(() => _virtualMachine.RefreshInstanceViewAsync());
            await refreshTask.ContinueWith(_ => _powerStateUpdating = false);
        }

        public async Task Update() 
            => await _virtualMachine.RefreshInstanceViewAsync();
    }
}