using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DistributedSystems.API.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
    
    public class DbConnectionFactory : IDbConnectionFactory, IDisposable
    {
        private readonly IDbConnection _connection;

        public DbConnectionFactory(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetValue<string>("meme"));
            _connection.Open();
        }

        public IDbConnection GetDbConnection() 
            => _connection;

        public void Dispose()
            => _connection?.Dispose();
    }
}