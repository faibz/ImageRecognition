using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DistributedSystems.WorkerManager.WorkerManager
{
    internal class WorkerQueueEvaluator
    {
        internal static async Task<WorkerAction> AdviseAction(int activeWorkerCount, int totalWorkerCount, long queueMessageCount, int targetSla, int minimumWorkerCount, HttpClient httpClient)
        {
            if (activeWorkerCount < minimumWorkerCount) return WorkerAction.Add;
            if (activeWorkerCount == minimumWorkerCount && queueMessageCount == 0) return WorkerAction.Nothing;
            if (activeWorkerCount > minimumWorkerCount && queueMessageCount == 0) return WorkerAction.Remove;

            var result = await (await httpClient.GetAsync("Data/ImageProcessingTime")).Content.ReadAsStringAsync();
            var timespan = new List<TimeSpan> { JsonConvert.DeserializeObject<TimeSpan>(result) };
            timespan.Add(JsonConvert.DeserializeObject<TimeSpan>(await (await httpClient.GetAsync("Data/CompoundImageProcessingTime")).Content.ReadAsStringAsync()));
            var averageProcessingTime = new TimeSpan(Convert.ToInt64(timespan.Average(time => time.Ticks))).TotalSeconds;

            if (averageProcessingTime > targetSla + 1 && activeWorkerCount < totalWorkerCount) return WorkerAction.Add;
            if (averageProcessingTime < targetSla - 1 && activeWorkerCount > minimumWorkerCount) return WorkerAction.Remove;

            return WorkerAction.Nothing;
        }
    }
}
