using Microsoft.Azure.Management.Compute.Fluent;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager
{
    public interface IWorker
    {
        Task TurnOn();
        Task TurnOff();
    }

    public class Worker : IWorker
    {
        private readonly IVirtualMachine _virtualMachine;

        public Worker(IVirtualMachine virtualMachine)
        {
            _virtualMachine = virtualMachine;
        }

        public bool PowerState { get; private set; } = false;

        public async Task TurnOn()
        {
            if (PowerState) return;

            await _virtualMachine.StartAsync();

            PowerState = _virtualMachine.PowerState.Value == "Starting" || _virtualMachine.PowerState.Value == "Running";
        }

        public async Task TurnOff()
        {
            if (!PowerState) return;

            await _virtualMachine.PowerOffAsync();

            PowerState = !(_virtualMachine.PowerState.Value == "Stopping" || _virtualMachine.PowerState.Value == "Stopped");
        }
    }
}