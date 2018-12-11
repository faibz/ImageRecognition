using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace DistributedSystems.WorkerManager.WorkerManager
{
    internal class WorkerQueueEvaluator
    {
        internal static async Task<WorkerAction> AdviseAction(int activeWorkerCount, int totalWorkerCount, long queueMessageCount, int targetSla, int minimumWorkerCount, HttpClient httpClient)
        {
            if (activeWorkerCount < minimumWorkerCount) return WorkerAction.Add;
            if (activeWorkerCount == minimumWorkerCount && queueMessageCount == 0) return WorkerAction.Nothing;
            if (activeWorkerCount > minimumWorkerCount && queueMessageCount == 0) return WorkerAction.Remove;

            var responseSingleImages = await Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .RetryAsync(3)
                .ExecuteAsync(async () =>  await httpClient.GetAsync("Data/ImageProcessingTime"));

            var responseCompoundImages = await Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .RetryAsync(3)
                .ExecuteAsync(async () => await httpClient.GetAsync("Data/CompoundImageProcessingTime"));

            string resultSingleImages;
            string resultCompoundImages;

            try
            {
                resultSingleImages = await responseSingleImages.Content.ReadAsStringAsync();
                resultCompoundImages = await responseCompoundImages.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return WorkerAction.Nothing;
            }

            var timespan = new List<TimeSpan>
            {
                JsonConvert.DeserializeObject<TimeSpan>(resultSingleImages),
                JsonConvert.DeserializeObject<TimeSpan>(resultCompoundImages)
            };
            var averageProcessingTime = new TimeSpan(Convert.ToInt64(timespan.Average(time => time.Ticks))).TotalSeconds;

            if (averageProcessingTime > targetSla + 1 && activeWorkerCount < totalWorkerCount) return WorkerAction.Add;
            if (averageProcessingTime < targetSla - 1 && activeWorkerCount > minimumWorkerCount) return WorkerAction.Remove;

            return WorkerAction.Nothing;
        }
    }
}
