using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DistributedSystems.API.Factories;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImageMappingsRepository
    {
        Task<IList<Guid>> GetImageIdsByCompoundImageId(Guid compoundImageId);
    }

    public class CompoundImageMappingsRepository : ICompoundImageMappingsRepository
    {
        private readonly IDbConnection _connection;

        public CompoundImageMappingsRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<IList<Guid>> GetImageIdsByCompoundImageId(Guid compoundImageId)
        {
            var imageIds = await _connection.QueryAsync<Guid>("SELECT [ImageId] FROM [dbo].[CompoundImageMappings] WHERE [CompoundImageId] = @CompoundImageId", new { CompoundImageId = compoundImageId });

            return imageIds.AsList();
        }
    }
}