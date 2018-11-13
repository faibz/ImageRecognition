using Microsoft.Azure.Management.Compute.Fluent;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager
{
    public class Worker
    {
        private readonly IVirtualMachine _virtualMachine;

        public Worker(IVirtualMachine virtualMachine)
        {
            _virtualMachine = virtualMachine;
        }

        public bool Active { get; private set; } = false;

        public async Task TurnOn()
        {
            if (Active) return;

            Active = true;

            await _virtualMachine.StartAsync();
        }

        public async Task ShutDown()
        {
            if (!Active) return;

            Active = false;

            await _virtualMachine.PowerOffAsync();
        }
    }
}