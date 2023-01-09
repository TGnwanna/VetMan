using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class editedBookingGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingGroups_ProductTypes_ProductTypeId",
                table: "BookingGroups");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "BookingGroups",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingGroups_ProductTypeId",
                table: "BookingGroups",
                newName: "IX_BookingGroups_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CustomerBookingPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingGroups_Products_ProductId",
                table: "BookingGroups",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingGroups_Products_ProductId",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CustomerBookingPayments");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "BookingGroups",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingGroups_ProductId",
                table: "BookingGroups",
                newName: "IX_BookingGroups_ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingGroups_ProductTypes_ProductTypeId",
                table: "BookingGroups",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }
    }
}
