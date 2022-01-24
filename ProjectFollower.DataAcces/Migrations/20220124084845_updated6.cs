using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentUrl",
                table: "CompanyDocuments");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CompanyDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CompanyDocuments");

            migrationBuilder.AddColumn<string>(
                name: "DocumentUrl",
                table: "CompanyDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
