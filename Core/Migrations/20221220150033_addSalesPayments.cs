using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addSalesPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductActivity",
                table: "ProductLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SellingPrice",
                table: "ProductLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "CommonDropDowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DropdownKey = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonDropDowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonDropDowns_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<int>(type: "int", nullable: false),
                    SalesLogId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetails_ProductInventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_SalesLogs_SalesLogId",
                        column: x => x.SalesLogId,
                        principalTable: "SalesLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMeans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonDropDownId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMeans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMeans_CommonDropDowns_CommonDropDownId",
                        column: x => x.CommonDropDownId,
                        principalTable: "CommonDropDowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentMeans_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<double>(type: "float", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    DepositType = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalesLogsId = table.Column<int>(type: "int", nullable: false),
                    PaymentMeansId = table.Column<int>(type: "int", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPayments_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesPayments_PaymentMeans_PaymentMeansId",
                        column: x => x.PaymentMeansId,
                        principalTable: "PaymentMeans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPayments_SalesLogs_SalesLogsId",
                        column: x => x.SalesLogsId,
                        principalTable: "SalesLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonDropDowns_CompanyBranchId",
                table: "CommonDropDowns",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMeans_CommonDropDownId",
                table: "PaymentMeans",
                column: "CommonDropDownId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMeans_CompanyBranchId",
                table: "PaymentMeans",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_InventoryId",
                table: "SaleDetails",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SalesLogId",
                table: "SaleDetails",
                column: "SalesLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPayments_CompanyBranchId",
                table: "SalesPayments",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPayments_PaymentMeansId",
                table: "SalesPayments",
                column: "PaymentMeansId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPayments_SalesLogsId",
                table: "SalesPayments",
                column: "SalesLogsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "SalesPayments");

            migrationBuilder.DropTable(
                name: "PaymentMeans");

            migrationBuilder.DropTable(
                name: "CommonDropDowns");

            migrationBuilder.DropColumn(
                name: "ProductActivity",
                table: "ProductLogs");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "ProductLogs");
        }
    }
}
