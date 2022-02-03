using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "ProjectComments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "ProjectComments",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
