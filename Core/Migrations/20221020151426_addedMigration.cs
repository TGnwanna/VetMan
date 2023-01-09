using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookingPayments_AspNetUsers_UpdatedByUserid",
                table: "CustomerBookingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookings_AspNetUsers_CustomerId",
                table: "CustomerBookings");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserid",
                table: "CustomerBookingPayments",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerBookingPayments_UpdatedByUserid",
                table: "CustomerBookingPayments",
                newName: "IX_CustomerBookingPayments_UpdatedByUserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "CustomerBookings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePaid",
                table: "CustomerBookingPayments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "CustomerBookingPayments",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookingPayments_AspNetUsers_UpdatedByUserId",
                table: "CustomerBookingPayments",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookings_Customers_CustomerId",
                table: "CustomerBookings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookingPayments_AspNetUsers_UpdatedByUserId",
                table: "CustomerBookingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookings_Customers_CustomerId",
                table: "CustomerBookings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "CustomerBookingPayments",
                newName: "UpdatedByUserid");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerBookingPayments_UpdatedByUserId",
                table: "CustomerBookingPayments",
                newName: "IX_CustomerBookingPayments_UpdatedByUserid");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "CustomerBookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePaid",
                table: "CustomerBookingPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "CustomerBookingPayments",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookingPayments_AspNetUsers_UpdatedByUserid",
                table: "CustomerBookingPayments",
                column: "UpdatedByUserid",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookings_AspNetUsers_CustomerId",
                table: "CustomerBookings",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
