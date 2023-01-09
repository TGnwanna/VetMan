using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addedProductLogAndSalesLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    MeasurementunitId = table.Column<int>(type: "int", nullable: true),
                    ShopCategoryId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventories_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductInventories_Measurementunits_MeasurementunitId",
                        column: x => x.MeasurementunitId,
                        principalTable: "Measurementunits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductInventories_ShopCategories_ShopCategoryId",
                        column: x => x.ShopCategoryId,
                        principalTable: "ShopCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductInventories_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPaid = table.Column<double>(type: "float", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesLogs_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductInventoryId = table.Column<int>(type: "int", nullable: false),
                    Qauntity = table.Column<int>(type: "int", nullable: false),
                    OldQauntity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLogs_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductLogs_ProductInventories_ProductInventoryId",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_CompanyBranchId",
                table: "ProductInventories",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_MeasurementunitId",
                table: "ProductInventories",
                column: "MeasurementunitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_ShopCategoryId",
                table: "ProductInventories",
                column: "ShopCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_SupplierId",
                table: "ProductInventories",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLogs_CompanyBranchId",
                table: "ProductLogs",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLogs_ProductInventoryId",
                table: "ProductLogs",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLogs_UserId",
                table: "ProductLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLogs_CompanyBranchId",
                table: "SalesLogs",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLogs_CustomerId",
                table: "SalesLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLogs_UserId",
                table: "SalesLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLogs");

            migrationBuilder.DropTable(
                name: "SalesLogs");

            migrationBuilder.DropTable(
                name: "ProductInventories");
        }
    }
}
