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
        Task<bool> InsertImage(Image image);
        Task<bool> UpdateImageStatus(Guid imageId, ImageStatus imageStatus);
        Task<string> GetImageKeyById(Guid imageId);
        Task<string> GetLocationById(Guid imageId);

    }

    public class ImagesRepository : IImagesRepository
    {
        private readonly IDbConnection _connection;

        public ImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<bool> InsertImage(Image image)
        {
            try
            {
                await _connection.ExecuteAsync("INSERT INTO [dbo].[Images] ([Id], [Location], [UploadedDate], [Status], [ImageKey]) VALUES (@Id, @Location, @UploadedDate, @Status, @ImageKey)", new { image.Id, image.Location, image.UploadedDate, image.Status, image.ImageKey });

                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        public async Task<bool> UpdateImageStatus(Guid imageId, ImageStatus imageStatus)
        {
            try
            {
                await _connection.ExecuteAsync("UPDATE [dbo].[Images] SET [Status] = @Status WHERE [Id] = @Id",
                   new { Id = imageId, Status = imageStatus });
                return true;
            }
            catch (Exception)
            { }

            return false;
            
        }

        public async Task<string> GetImageKeyById(Guid imageId) 
            => await _connection.QueryFirstAsync<string>("SELECT [ImageKey] FROM [dbo].[Images] WHERE [Id] = @Id", new { Id = imageId });

        public async Task<string> GetLocationById(Guid imageId)
            => await _connection.QueryFirstAsync<string>("SELECT [Location] FROM [dbo].[Images] WHERE [Id] = @Id", new { Id = imageId });
    }
}
