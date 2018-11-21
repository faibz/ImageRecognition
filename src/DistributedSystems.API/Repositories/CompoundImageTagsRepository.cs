using DistributedSystems.API.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using DistributedSystems.API.Factories;
using Dapper;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImageTagsRepository
    {
        Task InsertCompoundImageTag(Guid compoundImageId, Tag tag);
    }

    public class CompoundImageTagsRepository : ICompoundImageTagsRepository
    {
        private readonly IDbConnection _connection;

        public CompoundImageTagsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.GetDbConnection();
        }

        public async Task InsertCompoundImageTag(Guid compoundImageId, Tag tag) 
            => await _connection.ExecuteAsync("INSERT INTO [dbo].[CompoundImageTags] ([CompoundImageId], [Tag], [Confidence]) VALUES (@CompoundImageId, @Tag, @Confidence)", new { CompoundImageId = compoundImageId, Tag = tag.Name, Confidence = tag.Confidence});
    }
}