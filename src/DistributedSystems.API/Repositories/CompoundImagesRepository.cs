using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImagesRepository
    {
        Task InsertCompoundImage(CompoundImage compoundImage);
        Task<IList<CompoundImage>> GetByMapId(Guid mapId);
    }

    public class CompoundImagesRepository : ICompoundImagesRepository
    {
        private readonly IDbConnection _connection;

        public CompoundImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task InsertCompoundImage(CompoundImage compoundImage)
        {
            await _connection.ExecuteAsync("INSERT INTO [dbo].[CompoundImages] ([Id], [MapId]) VALUES (@Id, @MapId)", new { compoundImage.Id, compoundImage.MapId });
        }

        public async Task<IList<CompoundImage>> GetByMapId(Guid mapId)
        {
            var compoundImages = await _connection.QueryAsync<Models.DTOs.CompoundImage>("SELECT [CompoundImageId], [MapId] FROM [dbo].[CompoundImages] WHERE [MapId] = @MapId", new { MapId = mapId });

            return compoundImages.Select(img => (CompoundImage) img).ToList();
        }
    }
}