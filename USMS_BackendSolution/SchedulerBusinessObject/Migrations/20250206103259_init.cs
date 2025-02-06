using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchedulerBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", maxLength: 6, nullable: false),
                    Location = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    isOnline = table.Column<bool>(type: "BIT", nullable: false),
                    OnlineURL = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
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
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSubjectId = table.Column<int>(type: "INT", nullable: false),
                    SlotId = table.Column<int>(type: "INT", nullable: false),
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", maxLength: 6, nullable: false),
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
                columns: new[] { "RoomId", "CreateAt", "Location", "OnlineURL", "Status", "UpdateAt", "isOnline" },
                values: new object[,]
                {
                    { "G304", new DateTime(2025, 2, 6, 17, 32, 58, 681, DateTimeKind.Local).AddTicks(4781), "Grammar Room 304", null, 1, new DateTime(2025, 2, 6, 17, 32, 58, 681, DateTimeKind.Local).AddTicks(4791), false },
                    { "R.ON", new DateTime(2025, 2, 6, 17, 32, 58, 681, DateTimeKind.Local).AddTicks(4793), "Online", "https://meet.google.com/koi-kghw-tsy", 1, new DateTime(2025, 2, 6, 17, 32, 58, 681, DateTimeKind.Local).AddTicks(4794), true }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSchedule");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "TimeSlot");
        }
    }
}
