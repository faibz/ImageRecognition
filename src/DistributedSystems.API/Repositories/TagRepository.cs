using Dapper;
using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DistributedSystems.API.Controllers
{
    public interface ITagsRepository
    {
        Task InsertImageTag(Guid imageId, Tag tag);
    }

    public class TagsRepository : ITagsRepository
    {
        private readonly IDbConnection _connection;

        public TagsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.GetDbConnection();
        }

        public async Task InsertImageTag(Guid imageId, Tag tag)
        {
            await _connection.ExecuteAsync("INSERT INTO [dbo].[ImageTags] ([ImageId], ) VALUES ()");
        }
    }
}