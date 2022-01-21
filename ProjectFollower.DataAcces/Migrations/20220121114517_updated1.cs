using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentsUrls",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "DocumentsUrls",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
