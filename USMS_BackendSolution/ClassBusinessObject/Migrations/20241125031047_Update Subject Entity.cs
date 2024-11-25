using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "MLN122",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 10, 10, 46, 578, DateTimeKind.Local).AddTicks(2959), true, new DateTime(2024, 11, 25, 10, 10, 46, 578, DateTimeKind.Local).AddTicks(2960) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "PRM392",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 10, 10, 46, 577, DateTimeKind.Local).AddTicks(3773), true, new DateTime(2024, 11, 25, 10, 10, 46, 578, DateTimeKind.Local).AddTicks(2769) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "PRN231",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 10, 10, 46, 578, DateTimeKind.Local).AddTicks(2957), true, new DateTime(2024, 11, 25, 10, 10, 46, 578, DateTimeKind.Local).AddTicks(2958) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "MLN122",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3460), false, new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3461) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "PRM392",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3445), false, new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3457) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: "PRN231",
                columns: new[] { "CreatedAt", "IsAvailable", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3459), false, new DateTime(2024, 11, 25, 9, 41, 10, 948, DateTimeKind.Local).AddTicks(3459) });
        }
    }
}
