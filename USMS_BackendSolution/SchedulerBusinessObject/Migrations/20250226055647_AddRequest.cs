using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class AddRequest : Migration
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
                    RequestTeacher = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    ScheduleId = table.Column<int>(type: "INT", nullable: false),
                    IsChangeTeacher = table.Column<bool>(type: "bit", nullable: false),
                    AlternateTeacher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G201",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8664), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8677) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G202",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8680), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8681) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G203",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8683), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8684) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G204",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8686), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8687) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G205",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8688), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8689) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G301",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8691), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8692) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G302",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8693), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8694) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G303",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8696), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8697) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G304",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8698), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8699) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G305",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8701), new DateTime(2025, 2, 26, 12, 56, 47, 680, DateTimeKind.Local).AddTicks(8702) });

            migrationBuilder.CreateIndex(
                name: "IX_RequestSchedule_ScheduleId",
                table: "RequestSchedule",
                column: "ScheduleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestSchedule");

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G201",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4358), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4368) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G202",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4370), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4371) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G203",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4372), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4373) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G204",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4374), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4375) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G205",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4376), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4376) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G301",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4378), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4378) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G302",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4379), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4380) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G303",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4381), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4381) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G304",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4382), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4383) });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: "G305",
                columns: new[] { "CreateAt", "UpdateAt" },
                values: new object[] { new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4384), new DateTime(2025, 2, 11, 19, 41, 18, 993, DateTimeKind.Local).AddTicks(4384) });
        }
    }
}
