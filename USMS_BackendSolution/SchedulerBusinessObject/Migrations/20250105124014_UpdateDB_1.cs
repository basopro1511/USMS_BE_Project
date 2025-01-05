using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB_1 : Migration
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
                    SubjectId = table.Column<string>(type: "NVARCHAR(10)", nullable: false),
                    SemesterId = table.Column<string>(type: "NVARCHAR(4)", nullable: false),
                    RoomId = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    Date = table.Column<DateOnly>(type: "DATE", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TIME(7)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TIME(7)", nullable: false),
                    TeacherId = table.Column<string>(type: "NVARCHAR(8)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedule", x => x.ExamScheduleId);
                });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G304",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 1, 5, 19, 40, 13, 336, DateTimeKind.Local).AddTicks(2585), new DateTime(2025, 1, 5, 19, 40, 13, 336, DateTimeKind.Local).AddTicks(2593) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "R.ON",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 1, 5, 19, 40, 13, 336, DateTimeKind.Local).AddTicks(2596), new DateTime(2025, 1, 5, 19, 40, 13, 336, DateTimeKind.Local).AddTicks(2597) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSchedule");

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G304",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2024, 12, 2, 10, 33, 22, 46, DateTimeKind.Local).AddTicks(4802), new DateTime(2024, 12, 2, 10, 33, 22, 46, DateTimeKind.Local).AddTicks(4812) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "R.ON",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2024, 12, 2, 10, 33, 22, 46, DateTimeKind.Local).AddTicks(4813), new DateTime(2024, 12, 2, 10, 33, 22, 46, DateTimeKind.Local).AddTicks(4814) });
        }
    }
}
