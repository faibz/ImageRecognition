using System.Threading.Tasks;
using System.Data;
using Dapper;
using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System;

namespace DistributedSystems.API.Repositories
{
    public interface IMapsRepository
    {
        Task<Map> InsertMap(Map map);
        Task<Map> GetMapById(Guid mapId);
        Task<MapImagePart> GetMapImagePartByIdAndLocation(Guid mapId, int x, int y);
        Task<MapImagePart> InsertMapImagePart(MapImagePart mapImagePart);
        Task<MapImagePart> GetMapImagePartByImageId(Guid imageId);
    }

    public class MapsRepository : IMapsRepository
    {
        private readonly IDbConnection _connection;

        public MapsRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<Map> InsertMap(Map map)
        {
            await _connection.ExecuteAsync(
                "INSERT INTO [dbo].[Maps] ([Id], [ColumnCount], [RowCount]) VALUES (@Id, @ColumnCount, @RowCount)",
                new {map.Id, map.ColumnCount, map.RowCount});

            return map;
        }

        public async Task<Map> GetMapById(Guid mapId)
        {
            var map = await _connection.QueryFirstAsync<Models.DTOs.Map>(
                "SELECT TOP 1 [Id], [ColumnCount], [RowCount] FROM [dbo].[Maps] WHERE [Id] = @MapId",
                new { MapId = mapId });

            return (Map) map;
        }

        public async Task<MapImagePart> GetMapImagePartByIdAndLocation(Guid mapId, int x, int y)
        {
            var mapImagePart = await _connection.QuerySingleOrDefaultAsync<Models.DTOs.MapImagePart>(
                "SELECT TOP 1 [MapId], [ImageId], [CoordinateX], [CoordinateY] FROM [dbo].[MapImageParts] WHERE [MapId] = @MapId AND [CoordinateX] = @CoordinateX AND [CoordinateY] = @CoordinateY",
                new { MapId = mapId, CoordinateX = x, CoordinateY = y });

            var bob =  mapImagePart == null ? null : (MapImagePart) mapImagePart;

            return bob;
        }

        public async Task<MapImagePart> InsertMapImagePart(MapImagePart mapImagePart)
        {
            await _connection.ExecuteAsync("INSERT INTO [dbo].[MapImageParts] ([MapId], [ImageId], [CoordinateX], [CoordinateY]) VALUES (@MapId, @ImageId, @CoordinateX, @CoordinateY)", new { mapImagePart.MapId, mapImagePart.ImageId, mapImagePart.CoordinateX, mapImagePart.CoordinateY });

            return mapImagePart;
        }

        public async Task<MapImagePart> GetMapImagePartByImageId(Guid imageId)
        {
            var mapImagePart = await _connection.QueryFirstOrDefaultAsync<Models.DTOs.MapImagePart>("SELECT TOP 1 [MapId], [ImageId], [CoordinateX], [CoordinateY] FROM [dbo].[MapImageParts] WHERE [ImageId] = @ImageId", new { ImageId = imageId });

            return (MapImagePart) mapImagePart;
        }
    }
}