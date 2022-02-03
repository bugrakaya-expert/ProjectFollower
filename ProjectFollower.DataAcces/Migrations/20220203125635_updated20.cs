using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ProjectComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_ApplicationUserId",
                table: "ProjectComments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_AspNetUsers_ApplicationUserId",
                table: "ProjectComments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_AspNetUsers_ApplicationUserId",
                table: "ProjectComments");

            migrationBuilder.DropIndex(
                name: "IX_ProjectComments_ApplicationUserId",
                table: "ProjectComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ProjectComments");
        }
    }
}
