using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201812112115, "Expand ImageTags Tag Column")]
    public class ExpandImageTagsTagColumn : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201812112115_ExpandImageTagsTagColumn.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201812112115_ExpandImageTagsTagColumn.sql"));
        }
    }
}
