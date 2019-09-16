using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811121314, "Create WorkerClientVersions table.")]
    public class CreateWorkerClientVersionsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811121314_CreateWorkerClientVersionsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811121314_CreateWorkerClientVersionsTable.sql"));
        }
    }
}
