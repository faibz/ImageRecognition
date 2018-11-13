using System.ComponentModel;
using System.Configuration.Install;

namespace DistributedSystems.WorkerManager
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
