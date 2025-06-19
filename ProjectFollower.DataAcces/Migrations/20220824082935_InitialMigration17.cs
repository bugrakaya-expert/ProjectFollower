using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class InitialMigration17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sorting",
                table: "SchedulerPriority",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sorting",
                table: "SchedulerPriority");
        }
    }
}
