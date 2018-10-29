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
                await _connection.ExecuteAsync("INSERT INTO [dbo].[Images] ([Id], [Location], [UploadedDate], [Status]) VALUES (@Id, @Location, @UploadedDate, @Status)", new { image.Id, image.Location, image.UploadedDate, image.Status });

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
            => await _connection.QueryFirstAsync<string>("SELECT [Key] FROM [dbo].[Images] WHERE [Id] = @Id", new { Id = imageId });
    }
}
