using System.Threading.Tasks;
using System.Data;
using Dapper;
using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;

namespace DistributedSystems.API.Repositories
{
    public interface IMapRepository
    {
        Task<Map> InsertMap(Map map);
    }

    public class MapRepository : IMapRepository
    {
        private readonly IDbConnection _connection;

        public MapRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<Map> InsertMap(Map map)
        {
            await _connection.ExecuteAsync(
                "INSERT INTO [dbo].[Maps] ([Id], [ColumnCount], [RowCount]) VALUES (@Id, @ColumnCount, @RowCount",
                new {map.Id, map.ColumnCount, map.RowCount});

            return map;
        }
    }
}