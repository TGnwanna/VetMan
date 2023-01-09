using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class vaccineModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVaccines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVaccines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVaccines_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVaccines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaccineSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDelivered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SmsSubscribed = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribed = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccineSubscriptions_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VaccineSubscriptions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccineSubscriptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVaccines_CompanyBranchId",
                table: "ProductVaccines",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVaccines_ProductId",
                table: "ProductVaccines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineSubscriptions_CompanyBranchId",
                table: "VaccineSubscriptions",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineSubscriptions_CustomerId",
                table: "VaccineSubscriptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineSubscriptions_ProductId",
                table: "VaccineSubscriptions",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVaccines");

            migrationBuilder.DropTable(
                name: "VaccineSubscriptions");
        }
    }
}
