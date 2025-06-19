using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class InitialMigration16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InfoVerModal",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfoVerModal",
                table: "AspNetUsers");
        }
    }
}
