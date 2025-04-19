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
                    table.ForeignKey(
                        name: "FK_RequestSchedule_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomId", "CreateAt", "Location", "Status", "UpdateAt" },
                values: new object[,]
                {
                    { "G201", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4632), "Grammar Room 201", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4644) },
                    { "G202", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4647), "Grammar Room 202", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4648) },
                    { "G203", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4650), "Grammar Room 203", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4651) },
                    { "G204", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4653), "Grammar Room 204", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4653) },
                    { "G205", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4655), "Grammar Room 205", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4656) },
                    { "G301", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4658), "Grammar Room 301", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4658) },
                    { "G302", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4660), "Grammar Room 302", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4661) },
                    { "G303", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4662), "Grammar Room 303", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4663) },
                    { "G304", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4664), "Grammar Room 304", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4665) },
                    { "G305", new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4666), "Grammar Room 305", 1, new DateTime(2025, 4, 20, 3, 7, 17, 37, DateTimeKind.Local).AddTicks(4667) }
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
                name: "IX_RequestSchedule_ScheduleId",
                table: "RequestSchedule",
                column: "ScheduleId");

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
