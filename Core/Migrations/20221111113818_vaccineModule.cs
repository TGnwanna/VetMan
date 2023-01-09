using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class vaccineModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "DeliveryComment",
            //    table: "CustomerBookings");

            //migrationBuilder.DropColumn(
            //    name: "IsAdmin",
            //    table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "VaccineModule",
                table: "CompanySettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VaccineModule",
                table: "CompanySettings");

            //migrationBuilder.AddColumn<string>(
            //    name: "DeliveryComment",
            //    table: "CustomerBookings",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsAdmin",
            //    table: "AspNetUsers",
            //    type: "bit",
            //    nullable: true);
        }
    }
}
