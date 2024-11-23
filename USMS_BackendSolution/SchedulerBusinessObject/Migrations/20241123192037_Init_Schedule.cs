using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchedulerBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Init_Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", maxLength: 6, nullable: false),
                    Location = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    isOnline = table.Column<bool>(type: "bit", nullable: false),
                    OnlineURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.SlotId);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSubjectId = table.Column<int>(type: "INT", nullable: false),
                    SlotId = table.Column<int>(type: "INT", nullable: false),
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", maxLength: 6, nullable: false),
                    TeacherId = table.Column<string>(type: "NVARCHAR(8)", maxLength: 8, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_TimeSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "TimeSlots",
                        principalColumn: "SlotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CreateAt", "Location", "OnlineURL", "Status", "UpdateAt", "isOnline" },
                values: new object[,]
                {
                    { "G304", new DateTime(2024, 11, 24, 2, 20, 37, 624, DateTimeKind.Local).AddTicks(3556), "Grammar Room 304", null, 1, new DateTime(2024, 11, 24, 2, 20, 37, 624, DateTimeKind.Local).AddTicks(3565), false },
                    { "R.ON", new DateTime(2024, 11, 24, 2, 20, 37, 624, DateTimeKind.Local).AddTicks(3568), "Online", "https://meet.google.com/koi-kghw-tsy", 1, new DateTime(2024, 11, 24, 2, 20, 37, 624, DateTimeKind.Local).AddTicks(3568), true }
                });

            migrationBuilder.InsertData(
                table: "TimeSlots",
                columns: new[] { "SlotId", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeOnly(9, 15, 0), new TimeOnly(7, 0, 0) },
                    { 2, new TimeOnly(11, 45, 0), new TimeOnly(9, 30, 0) },
                    { 3, new TimeOnly(15, 15, 0), new TimeOnly(13, 0, 0) },
                    { 4, new TimeOnly(17, 45, 0), new TimeOnly(15, 30, 0) },
                    { 5, new TimeOnly(20, 15, 0), new TimeOnly(18, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RoomId",
                table: "Schedules",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SlotId",
                table: "Schedules",
                column: "SlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "TimeSlots");
        }
    }
}
