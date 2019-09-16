using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811251644, "Add ProcessedDate to Images Table")]
    public class AddProcessedDateToImagesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811251644_AddProcessedDateToImagesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811251644_AddProcessedDateToImagesTable.sql"));
        }
    }
}
