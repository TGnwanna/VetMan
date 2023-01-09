using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addedCompanyToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "ProductTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CustomerBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "CustomerBookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CustomerBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "CustomerBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerBookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CustomerBookingPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "CustomerBookingPayments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CustomerBookingPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "CustomerBookingPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerBookingPayments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BookingGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "BookingGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "BookingGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "ExpectedCostPrice",
                table: "BookingGroups",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotalityRecorded",
                table: "BookingGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityArrived",
                table: "BookingGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityLeft",
                table: "BookingGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "BookingGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyBranchId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistered",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyBranches_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowVetManModule = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanySettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WalletHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MyProperty = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletHistories_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WalletHistories_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_CompanyBranchId",
                table: "ProductTypes",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyBranchId",
                table: "Products",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyBranchId",
                table: "Customers",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBookings_CompanyBranchId",
                table: "CustomerBookings",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBookingPayments_CompanyBranchId",
                table: "CustomerBookingPayments",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingGroups_CompanyBranchId",
                table: "BookingGroups",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyBranchId",
                table: "AspNetUsers",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatedById",
                table: "Companies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranches_CompanyId",
                table: "CompanyBranches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_CompanyBranchId",
                table: "WalletHistories",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_WalletId",
                table: "WalletHistories",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CompanyBranchId",
                table: "Wallets",
                column: "CompanyBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingGroups_CompanyBranches_CompanyBranchId",
                table: "BookingGroups",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookingPayments_CompanyBranches_CompanyBranchId",
                table: "CustomerBookingPayments",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBookings_CompanyBranches_CompanyBranchId",
                table: "CustomerBookings",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CompanyBranches_CompanyBranchId",
                table: "Customers",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CompanyBranches_CompanyBranchId",
                table: "Products",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_CompanyBranches_CompanyBranchId",
                table: "ProductTypes",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingGroups_CompanyBranches_CompanyBranchId",
                table: "BookingGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookingPayments_CompanyBranches_CompanyBranchId",
                table: "CustomerBookingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBookings_CompanyBranches_CompanyBranchId",
                table: "CustomerBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CompanyBranches_CompanyBranchId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CompanyBranches_CompanyBranchId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_CompanyBranches_CompanyBranchId",
                table: "ProductTypes");

            migrationBuilder.DropTable(
                name: "CompanySettings");

            migrationBuilder.DropTable(
                name: "WalletHistories");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "CompanyBranches");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_CompanyBranchId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyBranchId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyBranchId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBookings_CompanyBranchId",
                table: "CustomerBookings");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBookingPayments_CompanyBranchId",
                table: "CustomerBookingPayments");

            migrationBuilder.DropIndex(
                name: "IX_BookingGroups_CompanyBranchId",
                table: "BookingGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "CustomerBookings");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "CustomerBookings");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CustomerBookings");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "CustomerBookings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerBookings");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "CustomerBookingPayments");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "CustomerBookingPayments");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CustomerBookingPayments");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "CustomerBookingPayments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerBookingPayments");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "ExpectedCostPrice",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "MotalityRecorded",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "QuantityArrived",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "QuantityLeft",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "BookingGroups");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateRegistered",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Products",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
