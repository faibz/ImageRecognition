using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201810230928, "Add UploadedDate and Status to Images table.")]
    public class AddUploadedDateAndStatusToImagesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810230928_AddUploadedDateAndStatusToImagesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810230928_AddUploadedDateAndStatusToImagesTable.sql"));
        }
    }
}
