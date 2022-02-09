using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CompanyType_CompanyTypeId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CompanyType");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CompanyTypeId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CompanyTypeId",
                table: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyTypeId",
                table: "Customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CompanyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CompanyTypeId",
                table: "Customer",
                column: "CompanyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CompanyType_CompanyTypeId",
                table: "Customer",
                column: "CompanyTypeId",
                principalTable: "CompanyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
