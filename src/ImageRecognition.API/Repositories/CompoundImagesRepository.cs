using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ImageRecognition.API.Factories;
using ImageRecognition.Shared.Models;

namespace ImageRecognition.API.Repositories
{
    public interface ICompoundImagesRepository
    {
        Task InsertCompoundImage(CompoundImage compoundImage);
        Task<IList<CompoundImage>> GetByMapId(Guid mapId);
        Task UpdateCompoundImageProcessedDate(Guid compoundImageId, DateTime dateTime);
        Task<IList<CompoundImage>> GetRecentProcessedImageDates();
    }

    public class CompoundImagesRepository : ICompoundImagesRepository
    {
        private readonly IDbConnection _connection;

        public CompoundImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task InsertCompoundImage(CompoundImage compoundImage) 
            => await _connection.ExecuteAsync("INSERT INTO [dbo].[CompoundImages] ([Id], [MapId], [UploadedDate]) VALUES (@Id, @MapId, @UploadedDate)", new { compoundImage.Id, compoundImage.MapId, compoundImage.UploadedDate });

        public async Task<IList<CompoundImage>> GetByMapId(Guid mapId)
        {
            var compoundImages = await _connection.QueryAsync<Models.DTOs.CompoundImage>("SELECT [Id], [MapId], [UploadedDate], [ProcessedDate] FROM [dbo].[CompoundImages] WHERE [MapId] = @MapId", new { MapId = mapId });

            return compoundImages.Select(img => (CompoundImage) img).ToList();
        }

        public async Task UpdateCompoundImageProcessedDate(Guid compoundImageId, DateTime dateTime) 
            => await _connection.ExecuteAsync("UPDATE [dbo].[CompoundImages] SET [ProcessedDate] = @ProcessedDate WHERE [Id] = @Id", new { Id = compoundImageId, ProcessedDate = dateTime });

        public async Task<IList<CompoundImage>> GetRecentProcessedImageDates()
        {
            var images = await _connection.QueryAsync<Models.DTOs.CompoundImage>("SELECT TOP 10 [UploadedDate], [ProcessedDate] FROM [dbo].[CompoundImages] WHERE [ProcessedDate] IS NOT NULL ORDER BY [ProcessedDate] DESC");

            return images.Select(img => (CompoundImage) img).ToList();
        }
    }
}