using SimpleMigrations;
using System.IO;

namespace DistributedSystems.Migrations.Migrations
{
    [Migration(201812112120, "Expand CompoundImageTags Tag Column")]
    public class ExpandCompoundImageTagsTagColumn : Migration
    {
        protected override void Down()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Down\201812112120_ExpandCompoundImageTagsTagColumn.sql"));
        }

        protected override void Up()
        {
            Execute(File.ReadAllText(@"..\..\..\Migrations\Up\201812112120_ExpandCompoundImageTagsTagColumn.sql"));
        }
    }
}
