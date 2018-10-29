using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201810291131, "Add Key to Images Table.")]
    public class AddKeyToImagesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810291131_AddKeyToImagesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810291131_AddKeyToImagesTable.sql"));
        }
    }
}
