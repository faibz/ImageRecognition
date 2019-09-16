using System.IO;
using SimpleMigrations;

namespace ImageRecognition.Migrations.Migrations
{
    [Migration(201810230927, "Create Statuses table.")]
    public class CreateStatusesTable : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810230927_CreateStatusesTable.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810230927_CreateStatusesTable.sql"));
        }
    }
}
