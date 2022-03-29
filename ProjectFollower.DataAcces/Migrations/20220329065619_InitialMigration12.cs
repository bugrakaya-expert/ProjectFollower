using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class InitialMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "ResponsibleMeetings");

            migrationBuilder.AddColumn<string>(
                name: "MeetingsId",
                table: "ResponsibleMeetings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingsId",
                table: "ResponsibleMeetings");

            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "ResponsibleMeetings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
