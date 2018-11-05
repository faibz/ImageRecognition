namespace DistributedSystems.WorkerManager
{
    internal class WorkerQueueEvaluator
    {
        private static readonly int MessagesPerWorker = 4;

        internal static WorkerAction AdviseAction(int activeWorkerCount, long queueMessageCount)
        {
            var messagesPerWorker = queueMessageCount / activeWorkerCount;

            if (messagesPerWorker > MessagesPerWorker) return WorkerAction.Add;
            else if (messagesPerWorker < MessagesPerWorker) return WorkerAction.Remove;

            return WorkerAction.Nothing;
        }
    }
}
