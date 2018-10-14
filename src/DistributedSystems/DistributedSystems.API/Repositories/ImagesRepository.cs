using System.Threading.Tasks;
using DistributedSystems.API.Models;
using Dapper;
using DistributedSystems.API.Factories;
using System.Data;

namespace DistributedSystems.API.Repositories
{
    public interface IImagesRepository
    {
        Task<Image> InsertImage(Image image);
    }

    public class ImagesRepository : IImagesRepository
    {
        private readonly IDbConnection _connection;

        public ImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<Image> InsertImage(Image image)
        {
            await _connection.ExecuteAsync("INSERT INTO [dbo].[Images] ([Id], [Location]) VALUES (@Id, @Location", new { image.Id, image.Location });

            return image; //lol?
        }
    }
}
