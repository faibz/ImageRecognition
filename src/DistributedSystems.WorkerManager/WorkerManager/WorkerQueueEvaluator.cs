namespace DistributedSystems.WorkerManager
{
    internal class WorkerQueueEvaluator
    {
        private static readonly int IdealMessagesPerWorker = 4;

        internal static WorkerAction AdviseAction(int activeWorkerCount, long queueMessageCount)
        {
            if (activeWorkerCount == 0 && queueMessageCount > 0) return WorkerAction.Add;

            var messagesPerWorker = queueMessageCount / activeWorkerCount;
            
            if (messagesPerWorker > IdealMessagesPerWorker) return WorkerAction.Add;
            else if (messagesPerWorker < IdealMessagesPerWorker) return WorkerAction.Remove;

            return WorkerAction.Nothing;
        }
    }
}
