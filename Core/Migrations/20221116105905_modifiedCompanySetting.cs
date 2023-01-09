using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class modifiedCompanySetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowVetManModule",
                table: "CompanySettings",
                newName: "DOCBookingModule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOCBookingModule",
                table: "CompanySettings",
                newName: "ShowVetManModule");
        }
    }
}
