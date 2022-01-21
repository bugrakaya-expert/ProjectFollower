using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectFollower.DataAcces.Migrations
{
    public partial class updated4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDocuments_Customer_CustomersId",
                table: "CompanyDocuments");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDocuments_CustomersId",
                table: "CompanyDocuments");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "CompanyDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomersId",
                table: "CompanyDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDocuments_CustomersId",
                table: "CompanyDocuments",
                column: "CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDocuments_Customer_CustomersId",
                table: "CompanyDocuments",
                column: "CustomersId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
