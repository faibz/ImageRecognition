using Dapper;
using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DistributedSystems.API.Repositories
{
    public interface ITagsRepository
    {
        Task InsertImageTag(Guid imageId, Tag tag, Guid? mapId);
    }

    public class TagsRepository : ITagsRepository
    {
        private readonly IDbConnection _connection;

        public TagsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.GetDbConnection();
        }

        public async Task InsertImageTag(Guid imageId, Tag tag, Guid? mapId)
        {
            await _connection.ExecuteAsync(
                "INSERT INTO [dbo].[ImageTags] ([ImageId], [Tag], [Confidence], [MapId]) VALUES (@ImageId, @Tag, @Confidence, @MapId)",
                new {ImageId = imageId, Tag = tag.Name, Confidence = tag.Confidence, MapId = mapId});
        }
    }
}