using System;
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
        Task UpdateImageStatus(Guid imageId, ImageStatus imageStatus);
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
            await _connection.ExecuteAsync("INSERT INTO [dbo].[Images] ([Id], [Location], [UploadedDate], [Status]) VALUES (@Id, @Location, @UploadedDate, @Status)", new { image.Id, image.Location, image.UploadedDate, image.Status });

            return image;
        }

        public async Task UpdateImageStatus(Guid imageId, ImageStatus imageStatus)
        {
            await _connection.ExecuteAsync("UPDATE [dbo].[Images] SET [Status] = @Status WHERE [Id] = @Id",
                new {Id = imageId, Status = imageStatus});
        }
    }
}
