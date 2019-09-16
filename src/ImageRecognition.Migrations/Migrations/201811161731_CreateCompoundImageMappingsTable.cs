using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811161731, "Create CompoundImageMappings Table")]
    public class CreateCompoundImageMappingsTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811161731_CreateCompoundImageMappingsTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811161731_CreateCompoundImageMappingsTable.sql"));
        }
    }
}
