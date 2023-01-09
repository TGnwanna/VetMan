using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class UpdatedApplicationUserAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Paystacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gateway_response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    channel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    exp_month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    exp_year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    card_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reusable = table.Column<bool>(type: "bit", nullable: true),
                    signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authorization_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    access_code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paystacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paystacks_WalletHistories_WalletHistoryId",
                        column: x => x.WalletHistoryId,
                        principalTable: "WalletHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paystacks_WalletHistoryId",
                table: "Paystacks",
                column: "WalletHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paystacks");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
