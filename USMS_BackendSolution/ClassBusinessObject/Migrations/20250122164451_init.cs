using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    SemesterName = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    MajorId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    ClassId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SemesterId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    MajorId = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    Term = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "StudentInClass",
                columns: table => new
                {
                    StudentClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSubjectId = table.Column<int>(type: "INT", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInClass", x => x.StudentClassId);
                    table.ForeignKey(
                        name: "FK_StudentInClass_ClassSubject_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubject",
                        principalColumn: "ClassSubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Semester",
                columns: new[] { "SemesterId", "EndDate", "SemesterName", "StartDate", "Status" },
                values: new object[,]
                {
                    { "FA25", new DateOnly(2025, 11, 25), "Fall2025", new DateOnly(2025, 9, 5), 1 },
                    { "SP25", new DateOnly(2025, 3, 24), "Spring2025", new DateOnly(2025, 1, 5), 1 },
                    { "SU25", new DateOnly(2025, 8, 11), "Summer2025", new DateOnly(2025, 5, 8), 1 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "SubjectId", "CreatedAt", "Description", "MajorId", "NumberOfSlot", "Status", "SubjectName", "Term", "UpdatedAt" },
                values: new object[,]
                {
                    { "CEA201", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6463), "Introduction to computer architecture", "IT", 18, 1, "Computer Organization and Architecture", 1, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6463) },
                    { "DBI202", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6471), "Database concepts and design", "IT", 20, 1, "Database Systems", 3, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6471) },
                    { "LAB211", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6473), "Practice Object-Oriented Programming", "IT", 20, 1, "OOP with Java Lab", 3, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6473) },
                    { "MAD101", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6467), "Fundamentals of discrete math", "IT", 20, 1, "Discrete Mathematics", 2, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6467) },
                    { "MAE101", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6449), "Engineering mathematics fundamentals", "IT", 25, 1, "Mathematics for Engineering", 1, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6459) },
                    { "MAS291", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6476), "Introduction to statistics", "IT", 20, 1, "Statistics & Probability", 4, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6477) },
                    { "MLN111", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6481), "Core principles of Marxism-Leninism", null, 16, 2, "Philosophy of Marxism – Leninism", 8, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6482) },
                    { "NWC203c", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6469), "Introduction to networking concepts", "IT", 18, 1, "Computer Networking", 2, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6469) },
                    { "PRF192", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6461), "Learn the basics of programming", "IT", 20, 1, "Programming Fundamentals", 1, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6461) },
                    { "PRJ301", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6474), "Developing web apps using Java", "IT", 25, 1, "Java Web Application Development", 4, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6475) },
                    { "PRN231", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6480), "Developing cross-platform applications", "IT", 20, 0, "Building Cross-Platform Back-End Application With .NET", 7, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6480) },
                    { "SSL101c", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6465), "Skills for academic excellence", null, 15, 1, "Academic Skills for University Success", 1, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6465) },
                    { "SWP391", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6478), "Capstone project in software development", "IT", 30, 1, "Software Development Project", 5, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6478) },
                    { "WDU203c", new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6483), "Introduction to UI/UX principles", "IT", 18, 1, "UI/UX Design", 8, new DateTime(2025, 1, 22, 23, 44, 50, 567, DateTimeKind.Local).AddTicks(6484) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SemesterId",
                table: "ClassSubject",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SubjectId",
                table: "ClassSubject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInClass_ClassSubjectId",
                table: "StudentInClass",
                column: "ClassSubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentInClass");

            migrationBuilder.DropTable(
                name: "ClassSubject");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
