using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Semester",
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
                    table.PrimaryKey("PK_Semester", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MajorId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfSlot = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubject",
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
                    table.PrimaryKey("PK_ClassSubject", x => x.ClassSubjectId);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Semester_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semester",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Semester",
                columns: new[] { "SemesterId", "EndDate", "SemesterName", "StartDate", "Status" },
                values: new object[,]
                {
                    { "FA25", new DateOnly(2025, 11, 25), "Fall2025", new DateOnly(2025, 9, 5), true },
                    { "SP25", new DateOnly(2025, 3, 24), "Spring2025", new DateOnly(2025, 1, 5), true },
                    { "SU25", new DateOnly(2025, 8, 11), "Summer2025", new DateOnly(2025, 5, 8), true }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "SubjectId", "CreatedAt", "Description", "MajorId", "NumberOfSlot", "Status", "SubjectName", "Term", "UpdatedAt" },
                values: new object[,]
                {
                    { "MLN122", new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(8011), "Description", "CA", 16, 2, "Political economics of Marxism – Leninism", 6, new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(8012) },
                    { "PRM392", new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(7991), "Description", "IT", 20, 1, "Mobile Programing", 8, new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(8005) },
                    { "PRN231", new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(8009), "Description", "IT", 20, 0, "Building Cross-Platform Back-End Application With .NET", 7, new DateTime(2025, 1, 14, 1, 47, 35, 214, DateTimeKind.Local).AddTicks(8009) }
                });

            migrationBuilder.InsertData(
                table: "ClassSubject",
                columns: new[] { "ClassSubjectId", "ClassId", "CreatedAt", "SemesterId", "Status", "SubjectId" },
                values: new object[,]
                {
                    { 1, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "PRM392" },
                    { 2, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "PRN231" },
                    { 3, "SE1702", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SP25", true, "MLN122" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SemesterId",
                table: "ClassSubject",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SubjectId",
                table: "ClassSubject",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSubject");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
