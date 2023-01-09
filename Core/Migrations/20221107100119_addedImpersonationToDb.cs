using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addedImpersonationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Impersonations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuperAdminUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAdminId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateImpersonated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSession = table.Column<bool>(type: "bit", nullable: false),
                    DateSessionEnded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmTheRealUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impersonations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impersonations");
        }
    }
}
