using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811161733, "Create CompoundImageTags Table")]
    public class CreateCompoundImageTagsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811161733_CreateCompoundImageTagsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811161733_CreateCompoundImageTagsTable.sql"));
        }
    }
}
