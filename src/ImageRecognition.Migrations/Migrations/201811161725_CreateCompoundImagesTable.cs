using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811161725, "Create CompoundImages Table")]
    public class CreateCompoundImagesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811161725_CreateCompoundImagesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811161725_CreateCompoundImagesTable.sql"));
        }
    }
}
