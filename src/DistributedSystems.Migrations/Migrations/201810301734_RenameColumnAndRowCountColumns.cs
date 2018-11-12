using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201810301734, "Rename ColumnCnt And RowCnt Columns")]
    public class RenameColumnAndRowCountColumns : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201810301734_RenameColumnAndRowCountColumns.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201810301734_RenameColumnAndRowCountColumns.sql"));
        }
    }
}
