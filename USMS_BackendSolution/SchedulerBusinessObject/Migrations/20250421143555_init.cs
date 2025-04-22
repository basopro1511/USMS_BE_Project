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
                name: "RequestSchedule",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "NVARCHAR(16)", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    AlternativeTeacher = table.Column<string>(type: "NVARCHAR(16)", nullable: true),
                    OriginalDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    OriginalSlotId = table.Column<int>(type: "int", nullable: false),
                    OriginalRoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    NewDate = table.Column<DateOnly>(type: "DATE", nullable: true),
                    NewSlotId = table.Column<int>(type: "int", nullable: true),
                    NewRoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: true),
                    Reason = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    ReplyResponse = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestSchedule", x => x.RequestId);
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
                    SlotId = table.Column<int>(type: "INT", nullable: false),
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
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    TeacherId = table.Column<string>(type: "NVARCHAR(16)", nullable: true),
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
                    { "G201", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6127), "Grammar Room 201", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6136) },
                    { "G202", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6138), "Grammar Room 202", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6138) },
                    { "G203", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6139), "Grammar Room 203", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6140) },
                    { "G204", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6141), "Grammar Room 204", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6141) },
                    { "G205", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6143), "Grammar Room 205", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6143) },
                    { "G301", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6144), "Grammar Room 301", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6145) },
                    { "G302", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6175), "Grammar Room 302", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6176) },
                    { "G303", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6177), "Grammar Room 303", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6177) },
                    { "G304", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6178), "Grammar Room 304", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6179) },
                    { "G305", new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6180), "Grammar Room 305", 1, new DateTime(2025, 4, 21, 21, 35, 55, 398, DateTimeKind.Local).AddTicks(6181) }
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
                name: "RequestSchedule");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "TimeSlot");
        }
    }
}
