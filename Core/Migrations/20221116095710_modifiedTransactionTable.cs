using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class modifiedTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_CompanyBranches_CompanyBranchId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CompanyBranchId",
                table: "Transactions",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CompanyBranchId",
                table: "Transactions",
                newName: "IX_Transactions_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Transactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Companies_CompanyId",
                table: "Transactions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Companies_CompanyId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transactions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Transactions",
                newName: "CompanyBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CompanyId",
                table: "Transactions",
                newName: "IX_Transactions_CompanyBranchId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CompanyBranches_CompanyBranchId",
                table: "Transactions",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id");
        }
    }
}
