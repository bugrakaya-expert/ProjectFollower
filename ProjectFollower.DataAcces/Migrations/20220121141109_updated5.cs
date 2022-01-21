using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Business_BusinessId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_BusinessId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Customer_BusinessId",
                table: "Customer",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Business_BusinessId",
                table: "Customer",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
