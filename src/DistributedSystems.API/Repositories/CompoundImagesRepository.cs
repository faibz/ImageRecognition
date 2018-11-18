using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImagesRepository
    {
        Task InsertCompoundImage(CompoundImage compoundImage);
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
    }
}