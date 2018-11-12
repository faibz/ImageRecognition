using DistributedSystems.API.Adapters;
using DistributedSystems.API.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
{
    public interface IWorkerClientVersionsService
    {
        Task<WorkerClientVersion> GetWorkerClientVersion(string clientVersion);
        Task<WorkerClientVersion> GetLatestWorkerClient();
        Task<bool> ValidateUpdateKey(string updateKey);
        Task UpdateWorkerClient(byte[] clientData);
    }

    public class WorkerClientVersionsService : IWorkerClientVersionsService
    {
        private readonly IWorkerClientVersionsRepository _workerVersionsRepository;
        private readonly IFileStorageAdapter _fileStorageAdapter;
        private readonly string _storageContainerName;

        public WorkerClientVersionsService(IWorkerClientVersionsRepository workerVersionsRepository, IFileStorageAdapter fileStorageAdapter, IConfiguration configuration)
        {
            _workerVersionsRepository = workerVersionsRepository;
            _fileStorageAdapter = fileStorageAdapter;
            _storageContainerName = configuration.GetValue<string>("Azure:CloudBlobWorkerClientContainerName");
        }

        public async Task<WorkerClientVersion> GetWorkerClientVersion(string clientVersion)
            => await _workerVersionsRepository.GetWorkerByVersion(clientVersion);

        public async Task<WorkerClientVersion> GetLatestWorkerClient()
            => await _workerVersionsRepository.GetLatestWorkerClient();

        public async Task<bool> ValidateUpdateKey(string updateKey)
        {
            return true;
        }

        public async Task UpdateWorkerClient(byte[] clientData)
        {
            var memoryStream = new MemoryStream(clientData);
            var clientUpdate = new WorkerClientVersion(int.Parse((await _workerVersionsRepository.GetLatestWorkerClient()).Version) + 1);

            using (var md5 = MD5.Create())
                clientUpdate.Hash = BitConverter.ToString(md5.ComputeHash(memoryStream)).Replace("-", "").ToLower();

            memoryStream.Position = 0;
            clientUpdate.Location = await _fileStorageAdapter.UploadFile($"WorkerClient_v{clientUpdate.Version}.exe", memoryStream, _storageContainerName);

            if (string.IsNullOrEmpty(clientUpdate.Location)) return;

            await _workerVersionsRepository.InsertWorkerClientVersion(clientUpdate);
        }
    }
}