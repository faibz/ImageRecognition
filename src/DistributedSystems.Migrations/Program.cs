using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System.Data.SqlClient;

namespace DistributedSystems.Migrations
{
    public class Program
    {
        private static void Main(string[] args)
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
