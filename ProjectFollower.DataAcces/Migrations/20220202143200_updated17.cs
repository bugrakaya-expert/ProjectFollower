using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_AspNetUsers_ApplicationUserId1",
                table: "ProjectComments");

            migrationBuilder.DropIndex(
                name: "IX_ProjectComments_ApplicationUserId1",
                table: "ProjectComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ProjectComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "ProjectComments");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ProjectComments");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentTime",
                table: "ProjectComments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentTime",
                table: "ProjectComments");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "ProjectComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "ProjectComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ProjectComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_ApplicationUserId1",
                table: "ProjectComments",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_AspNetUsers_ApplicationUserId1",
                table: "ProjectComments",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
