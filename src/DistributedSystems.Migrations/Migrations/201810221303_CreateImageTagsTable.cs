using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201801221303, "Create ImageTags table.")]
    public class CreateImageTagsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201801221303_CreateImageTagsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201801221303_CreateImageTagsTable.sql"));
        }
    }
}
