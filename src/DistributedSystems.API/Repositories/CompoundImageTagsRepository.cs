using DistributedSystems.API.Models;
using System;
using System.Threading.Tasks;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImageTagsRepository
    {
        Task InsertCompoundImageTag(Guid compoundImageId, Tag tags);
    }
}