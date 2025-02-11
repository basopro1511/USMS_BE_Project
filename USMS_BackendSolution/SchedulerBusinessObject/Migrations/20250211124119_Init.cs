using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchedulerBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamSchedule",
                columns: table => new
                {
                    ExamScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterId = table.Column<string>(type: "NVARCHAR(4)", nullable: false),
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", nullable: true),
                    SubjectId = table.Column<string>(type: "NVARCHAR(10)", nullable: false),
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: true),
                    Date = table.Column<DateOnly>(type: "DATE", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TIME(7)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TIME(7)", nullable: false),
                    TeacherId = table.Column<string>(type: "NVARCHAR(8)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedule", x => x.ExamScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    Location = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Status = table.Column<int>(type: "INT", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "TIME(0)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TIME(0)", nullable: false),
                    Status = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.SlotId);
                });

            migrationBuilder.CreateTable(
                name: "StudentInExamSchedule",
                columns: table => new
                {
                    StudentExamId = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamScheduleId = table.Column<int>(type: "INT", nullable: false),
                    StudentId = table.Column<string>(type: "NVARCHAR(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInExamSchedule", x => x.StudentExamId);
                    table.ForeignKey(
                        name: "FK_StudentInExamSchedule_ExamSchedule_ExamScheduleId",
                        column: x => x.ExamScheduleId,
                        principalTable: "ExamSchedule",
                        principalColumn: "ExamScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSubjectId = table.Column<int>(type: "INT", nullable: false),
                    SlotId = table.Column<int>(type: "INT", nullable: false),
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    TeacherId = table.Column<string>(type: "NVARCHAR(8)", maxLength: 8, nullable: true),
                    Date = table.Column<DateOnly>(type: "DATE", nullable: false),
                    Status = table.Column<int>(type: "INT", nullable: false),
                    SlotNoInSubject = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedule_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_TimeSlot_SlotId",
                        column: x => x.SlotId,
                        principalTable: "TimeSlot",
                        principalColumn: "SlotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomId", "CreateAt", "Location", "Status", "UpdateAt" },
                values: new object[,]
                {
                    { "G201", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4358), "Grammar Room 201", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4368) },
                    { "G202", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4370), "Grammar Room 202", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4371) },
                    { "G203", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4372), "Grammar Room 203", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4373) },
                    { "G204", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4374), "Grammar Room 204", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4375) },
                    { "G205", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4376), "Grammar Room 205", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4376) },
                    { "G301", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4378), "Grammar Room 301", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4378) },
                    { "G302", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4379), "Grammar Room 302", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4380) },
                    { "G303", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4381), "Grammar Room 303", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4381) },
                    { "G304", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4382), "Grammar Room 304", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4383) },
                    { "G305", new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4384), "Grammar Room 305", 1, new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4384) }
                });

            migrationBuilder.InsertData(
                table: "TimeSlot",
                columns: new[] { "SlotId", "EndTime", "StartTime", "Status" },
                values: new object[,]
                {
                    { 1, new TimeOnly(9, 15, 0), new TimeOnly(7, 0, 0), 1 },
                    { 2, new TimeOnly(11, 45, 0), new TimeOnly(9, 30, 0), 1 },
                    { 3, new TimeOnly(15, 15, 0), new TimeOnly(13, 0, 0), 1 },
                    { 4, new TimeOnly(17, 45, 0), new TimeOnly(15, 30, 0), 1 },
                    { 5, new TimeOnly(20, 15, 0), new TimeOnly(18, 0, 0), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_RoomId",
                table: "Schedule",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SlotId",
                table: "Schedule",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInExamSchedule_ExamScheduleId",
                table: "StudentInExamSchedule",
                column: "ExamScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "StudentInExamSchedule");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropTable(
                name: "ExamSchedule");
        }
    }
}
