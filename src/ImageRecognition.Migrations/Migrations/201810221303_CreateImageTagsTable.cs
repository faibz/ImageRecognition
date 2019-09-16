using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201810221303, "Create ImageTags table.")]
    public class CreateImageTagsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810221303_CreateImageTagsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810221303_CreateImageTagsTable.sql"));
        }
    }
}
