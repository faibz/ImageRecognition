using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ImageRecognition.API.Factories;
using ImageRecognition.API.Models.DTOs;
using ImageRecognition.Shared.Models;

namespace ImageRecognition.API.Repositories
{
    public interface ITagsRepository
    {
        Task InsertImageTag(Guid imageId, Tag tag, Guid? mapId);
        Task<IList<Tag>> GetTagsByImageId(Guid imageId);
        Task<IList<Tag>> GetTagsByMapId(Guid mapId);
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

        public async Task<IList<Tag>> GetTagsByImageId(Guid imageId)
        {
            var dbTags = await _connection.QueryAsync<ImageTag>("SELECT [Tag], [Confidence] FROM [dbo].[ImageTags] WHERE [ImageId] = @ImageId", new { ImageId = imageId });

            return dbTags.Select(tag => (Tag) tag).ToList();
        }

        public async Task<IList<Tag>> GetTagsByMapId(Guid mapId)
        {
            var dbTags = await _connection.QueryAsync<ImageTag>("SELECT [Tag], [Confidence] FROM [dbo].[ImageTags] WHERE [MapId] = @MapId", new { MapId = mapId });

            return dbTags.Select(tag => (Tag) tag).ToList();
        }
    }
}
