using Microsoft.EntityFrameworkCore.Migrations;

namespace News.Api.Migrations
{
    public partial class NewsTypeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "News",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
