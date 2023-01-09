using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addedNullToAllFKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBranches_Companies_CompanyId",
                table: "CompanyBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Paystacks_WalletHistories_WalletHistoryId",
                table: "Paystacks");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletHistoryId",
                table: "Paystacks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyBranches",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBranches_Companies_CompanyId",
                table: "CompanyBranches",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Paystacks_WalletHistories_WalletHistoryId",
                table: "Paystacks",
                column: "WalletHistoryId",
                principalTable: "WalletHistories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBranches_Companies_CompanyId",
                table: "CompanyBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Paystacks_WalletHistories_WalletHistoryId",
                table: "Paystacks");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletHistoryId",
                table: "Paystacks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyBranches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyBranches_CompanyBranchId",
                table: "AspNetUsers",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBranches_Companies_CompanyId",
                table: "CompanyBranches",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paystacks_WalletHistories_WalletHistoryId",
                table: "Paystacks",
                column: "WalletHistoryId",
                principalTable: "WalletHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
