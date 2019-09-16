using System.Data.SqlClient;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace ImageRecognition.Migrations
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var migrationsAssembly = typeof(Program).Assembly;
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "ImageRecognition",
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
