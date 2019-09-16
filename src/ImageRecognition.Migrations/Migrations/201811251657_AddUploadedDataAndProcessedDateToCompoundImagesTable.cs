using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201811251657, "Add UploadedData and ProcessedDate to CompoundImages Table")]
    public class AddUploadedDataAndProcessedDateToCompoundImagesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201811251657_AddUploadedDataAndProcessedDateToCompoundImagesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201811251657_AddUploadedDataAndProcessedDateToCompoundImagesTable.sql"));
        }
    }
}
