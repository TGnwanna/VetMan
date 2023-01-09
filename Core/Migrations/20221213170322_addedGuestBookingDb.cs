using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
	public partial class addedGuestBookingDb : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ModuleCosts_CompanyModules_CompanyModuleId",
				table: "ModuleCosts");

			migrationBuilder.DropIndex(
				name: "IX_ModuleCosts_CompanyModuleId",
				table: "ModuleCosts");

			migrationBuilder.DropColumn(
				name: "CompanyModuleId",
				table: "ModuleCosts");

			//migrationBuilder.DropColumn(
			//	name: "IsGuest",
			//	table: "CustomerBookings");

			migrationBuilder.AddColumn<int>(
				name: "DepositType",
				table: "SalesLogs",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<Guid>(
				name: "GuestBookingId",
				table: "Paystacks",
				type: "uniqueidentifier",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "ModuleId",
				table: "ModuleCosts",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.CreateTable(
				name: "GuestBookings",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Quantity = table.Column<double>(type: "float", nullable: false),
					TotalPrice = table.Column<double>(type: "float", nullable: false),
					BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GuestBookings", x => x.Id);
					table.ForeignKey(
						name: "FK_GuestBookings_BookingGroups_BookingId",
						column: x => x.BookingId,
						principalTable: "BookingGroups",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_GuestBookings_CompanyBranches_CompanyBranchId",
						column: x => x.CompanyBranchId,
						principalTable: "CompanyBranches",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Paystacks_GuestBookingId",
				table: "Paystacks",
				column: "GuestBookingId");

			migrationBuilder.CreateIndex(
				name: "IX_GuestBookings_BookingId",
				table: "GuestBookings",
				column: "BookingId");

			migrationBuilder.CreateIndex(
				name: "IX_GuestBookings_CompanyBranchId",
				table: "GuestBookings",
				column: "CompanyBranchId");

			migrationBuilder.AddForeignKey(
				name: "FK_Paystacks_GuestBookings_GuestBookingId",
				table: "Paystacks",
				column: "GuestBookingId",
				principalTable: "GuestBookings",
				principalColumn: "Id");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Paystacks_GuestBookings_GuestBookingId",
				table: "Paystacks");

			migrationBuilder.DropTable(
				name: "GuestBookings");

			migrationBuilder.DropIndex(
				name: "IX_Paystacks_GuestBookingId",
				table: "Paystacks");

			migrationBuilder.DropColumn(
				name: "DepositType",
				table: "SalesLogs");

			migrationBuilder.DropColumn(
				name: "GuestBookingId",
				table: "Paystacks");

			migrationBuilder.DropColumn(
				name: "ModuleId",
				table: "ModuleCosts");

			migrationBuilder.AddColumn<Guid>(
				name: "CompanyModuleId",
				table: "ModuleCosts",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

			//migrationBuilder.AddColumn<bool>(
			//	name: "IsGuest",
			//	table: "CustomerBookings",
			//	type: "bit",
			//	nullable: false,
			//	defaultValue: false);

			migrationBuilder.CreateIndex(
				name: "IX_ModuleCosts_CompanyModuleId",
				table: "ModuleCosts",
				column: "CompanyModuleId");

			migrationBuilder.AddForeignKey(
				name: "FK_ModuleCosts_CompanyModules_CompanyModuleId",
				table: "ModuleCosts",
				column: "CompanyModuleId",
				principalTable: "CompanyModules",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
