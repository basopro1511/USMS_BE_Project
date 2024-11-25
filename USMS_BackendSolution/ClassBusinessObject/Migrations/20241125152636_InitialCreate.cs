using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    SemesterName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfSlot = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubjects",
                columns: table => new
                {
                    ClassSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SemesterId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubjects", x => x.ClassSubjectId);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "SemesterId", "EndDate", "SemesterName", "StartDate", "Status" },
                values: new object[,]
                {
                    { "FA25", new DateOnly(2025, 11, 25), "Fall2025", new DateOnly(2025, 9, 5), true },
                    { "SP25", new DateOnly(2025, 3, 24), "Spring2025", new DateOnly(2025, 1, 5), true },
                    { "SU25", new DateOnly(2025, 8, 11), "Summer2025", new DateOnly(2025, 5, 8), true }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "SubjectId", "CreatedAt", "Description", "NumberOfSlot", "Status", "SubjectName", "UpdatedAt" },
                values: new object[,]
                {
                    { "MLN122", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7273), "Description", 16, true, "Political economics of Marxism – Leninism", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7274) },
                    { "PRM392", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7256), "Description", 20, true, "Mobile Programing", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7268) },
                    { "PRN231", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7270), "Description", 20, true, "Building Cross-Platform Back-End Application With .NET", new DateTime(2024, 11, 25, 22, 26, 35, 632, DateTimeKind.Local).AddTicks(7271) }
                });

            migrationBuilder.InsertData(
                table: "ClassSubjects",
                columns: new[] { "ClassSubjectId", "ClassId", "CreatedAt", "SemesterId", "Status", "SubjectId" },
                values: new object[,]
                {
                    { 1, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "PRM392" },
                    { 2, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "PRN231" },
                    { 3, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "MLN122" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_SemesterId",
                table: "ClassSubjects",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_SubjectId",
                table: "ClassSubjects",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSubjects");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
