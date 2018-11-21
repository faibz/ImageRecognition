using DistributedSystems.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Factories;
using Dapper;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImageTagsRepository
    {
        Task InsertCompoundImageTag(Guid compoundImageId, Tag tag);
        Task<IList<CompoundImageTag>> GetTagsByCompoundImageId(Guid compoundImageId);
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

        public async Task<IList<CompoundImageTag>> GetTagsByCompoundImageId(Guid compoundImageId)
        {
            var dbTags = await _connection.QueryAsync<Models.DTOs.CompoundImageTag>("SELECT [CompoundImageId], [Tag], [Confidence] FROM [dbo].[CompoundImageTags] WHERE [CompoundImageId] = @CompoundImageId", new { CompoundImageId = compoundImageId });

            return dbTags.Select(tag => (CompoundImageTag)tag).ToList();
        }
    }
}