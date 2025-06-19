using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class InitialMigration18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SchedulerAuth",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchedulerAuth",
                table: "AspNetUsers");
        }
    }
}
