using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System;
using System.Data.SqlClient;

namespace DistributedSystems.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var migrationsAssembly = typeof(Program).Assembly;
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "DistributedSystems",
                IntegratedSecurity = true
            }.ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var dbProvider = new MssqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(migrationsAssembly, dbProvider);

                migrator.Load();
                migrator.MigrateToLatest();
            }
        }
    }
}
