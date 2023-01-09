using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class updatedCompanySubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsGuest",
            //    table: "CustomerBookings",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CompanyModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SubcriptionStatus = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyModules_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaystackSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PaystackSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaystackSubscriptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModuleCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    NoOfDays = table.Column<int>(type: "int", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleCosts_CompanyModules_CompanyModuleId",
                        column: x => x.CompanyModuleId,
                        principalTable: "CompanyModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaystackSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleCostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleOrderItems_ModuleCosts_ModuleCostId",
                        column: x => x.ModuleCostId,
                        principalTable: "ModuleCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleOrderItems_PaystackSubscriptions_PaystackSubscriptionId",
                        column: x => x.PaystackSubscriptionId,
                        principalTable: "PaystackSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_CompanyId",
                table: "CompanyModules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleCosts_CompanyModuleId",
                table: "ModuleCosts",
                column: "CompanyModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOrderItems_ModuleCostId",
                table: "ModuleOrderItems",
                column: "ModuleCostId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOrderItems_PaystackSubscriptionId",
                table: "ModuleOrderItems",
                column: "PaystackSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaystackSubscriptions_CompanyId",
                table: "PaystackSubscriptions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_UserId",
                table: "UserLoginLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleOrderItems");

            migrationBuilder.DropTable(
                name: "UserLoginLogs");

            migrationBuilder.DropTable(
                name: "ModuleCosts");

            migrationBuilder.DropTable(
                name: "PaystackSubscriptions");

            migrationBuilder.DropTable(
                name: "CompanyModules");

            //migrationBuilder.DropColumn(
            //    name: "IsGuest",
            //    table: "CustomerBookings");
        }
    }
}
