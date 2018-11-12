using System.ServiceModel;

namespace DistributedSystems.WorkerManager
{
    [ServiceContract]
    interface IManager
    {
        [OperationContract]
        int TotalWorkerCount();
        [OperationContract]
        int CurrentWorkerCount();
    }
}
