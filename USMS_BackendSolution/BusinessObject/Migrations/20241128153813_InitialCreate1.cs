using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", maxLength: 4, nullable: false),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.MajorId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR(8)", maxLength: 8, nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PersonalEmail = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(15)", maxLength: 15, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: "CE170288",
                column: "Name",
                value: "Nguyễn Quốc Hoàng");

            migrationBuilder.InsertData(
                table: "Major",
                columns: new[] { "MajorId", "MajorName" },
                values: new object[,]
                {
                    { "IB", "International Business" },
                    { "IT", "Information Technology" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Academic Staff" },
                    { 3, "Chairperson" },
                    { 4, "Lecture" },
                    { 5, "Student" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "PersonalEmail", "PhoneNumber", "RoleId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { "AD0001", new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(2046), "admin0001@university.edu", "Nguyen", "Admin", "Tuan", "hashedpassword3", "admin0001@gmail.edu", "0123456780", 1, 1, new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(2046) },
                    { "IB0001", new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(2044), "BTTIB0001.b@university.edu", "Tran", "B", "Thi", "hashedpassword2", "thi.b@gmail.com", "0987654321", 5, 1, new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(2044) },
                    { "IT0001", new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(1993), "ANVIT0001.a@university.edu", "Nguyen", "A", "Van", "hashedpassword1", "van.a@gmail.com", "0123456789", 5, 1, new DateTime(2024, 11, 28, 22, 38, 13, 496, DateTimeKind.Local).AddTicks(2003) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: "CE170288",
                column: "Name",
                value: "Nguyễn Quốcc Hoàng");
        }
    }
}
