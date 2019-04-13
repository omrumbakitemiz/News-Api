using Microsoft.EntityFrameworkCore.Migrations;

namespace News.Api.Migrations
{
    public partial class ImageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "News",
                newName: "image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "News",
                newName: "Images");
        }
    }
}
