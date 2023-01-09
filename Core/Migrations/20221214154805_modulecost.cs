using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class modulecost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ModuleCosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleCostId",
                table: "CompanyModules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_ModuleCostId",
                table: "CompanyModules",
                column: "ModuleCostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyModules_ModuleCosts_ModuleCostId",
                table: "CompanyModules",
                column: "ModuleCostId",
                principalTable: "ModuleCosts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyModules_ModuleCosts_ModuleCostId",
                table: "CompanyModules");

            migrationBuilder.DropIndex(
                name: "IX_CompanyModules_ModuleCostId",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "ModuleCosts");

            migrationBuilder.DropColumn(
                name: "ModuleCostId",
                table: "CompanyModules");
        }
    }
}
