using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201810151611, "Create MapImageParts table.")]
    public class CreateMapImagePartsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810151611_CreateMapImagePartsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810151611_CreateMapImagePartsTable.sql"));
        }
    }
}
