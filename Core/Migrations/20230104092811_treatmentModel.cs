using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class treatmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breeds_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    BreedId = table.Column<int>(type: "int", nullable: true),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialMarking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_CommonDropDowns_GenderId",
                        column: x => x.GenderId,
                        principalTable: "CommonDropDowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    CurrentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SMSAllowed = table.Column<bool>(type: "bit", nullable: false),
                    EmailAllowed = table.Column<bool>(type: "bit", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routines_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routines_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TreatmentHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccinationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvironmentalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedingHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralExamination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalExamination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysiologicalExamination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoryExamination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratorySamples = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoryResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifferentialDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefinitiveDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visits_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visits_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitTreatments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    TreatmentId = table.Column<int>(type: "int", nullable: true),
                    VisitId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTreatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitTreatments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitTreatments_CompanyBranches_CompanyBranchId",
                        column: x => x.CompanyBranchId,
                        principalTable: "CompanyBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitTreatments_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitTreatments_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GenderId",
                table: "Customers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_CompanyBranchId",
                table: "Breeds",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_BreedId",
                table: "Patients",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CompanyBranchId",
                table: "Patients",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CustomerId",
                table: "Patients",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_GenderId",
                table: "Patients",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_CompanyBranchId",
                table: "Routines",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_PatientId",
                table: "Routines",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_CompanyBranchId",
                table: "Treatments",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_CompanyBranchId",
                table: "Visits",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_CreatedById",
                table: "Visits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientId",
                table: "Visits",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_CompanyBranchId",
                table: "VisitTreatments",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_CreatedById",
                table: "VisitTreatments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_TreatmentId",
                table: "VisitTreatments",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_VisitId",
                table: "VisitTreatments",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CommonDropDowns_GenderId",
                table: "Customers",
                column: "GenderId",
                principalTable: "CommonDropDowns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CommonDropDowns_GenderId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "VisitTreatments");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropIndex(
                name: "IX_Customers_GenderId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Customers");
        }
    }
}
