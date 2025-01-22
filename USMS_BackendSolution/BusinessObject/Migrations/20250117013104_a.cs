using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 11, nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", maxLength: 4, nullable: false),
                    MajorName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false)
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
                    Email = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    PersonalEmail = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(15)", maxLength: 15, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    UserAvartar = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "NVARCHAR(8)", maxLength: 8, nullable: false),
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", maxLength: 4, nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "MajorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Student_User_StudentId",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "Name", "Phone" },
                values: new object[] { "CE170288", "Phong Điền, Cần Thơ", "Nguyễn Quốc Hoàng", "0333744591" });

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
                    { 2, "AcademicStaff" },
                    { 3, "Chairperson" },
                    { 4, "Teacher" },
                    { 5, "Student" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "DateOfBirth", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "PersonalEmail", "PhoneNumber", "RoleId", "Status", "UpdatedAt", "UserAvartar" },
                values: new object[,]
                {
                    { "IB0001", new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6041), new DateOnly(1990, 1, 1), "ThangNTIB0001@gmail.com", "Thắng", "Nguyễn", "Toàn", "123", "nqh@gmail.com", "0333744591", 5, 1, new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6042), "123" },
                    { "IB0002", new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6044), new DateOnly(1990, 1, 1), "AnLDIB0002@gmail.com", "Ân", "Lê", "Đức", "123", "nqh@gmail.com", "0333744591", 5, 1, new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6044), "123" },
                    { "IT0001", new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6022), new DateOnly(1990, 1, 1), "HoangNQIT0001@gmail.com", "Hoàng", "Nguyễn", "Quốc", "123", "nqh@gmail.com", "0333744591", 5, 1, new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6035), "123" },
                    { "IT0002", new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6038), new DateOnly(1990, 1, 1), "ThinhNTIT0002@gmail.com", "Thịnh", "Nguyễn", "Tuấn", "123", "nqh@gmail.com", "0333744591", 5, 1, new DateTime(2025, 1, 17, 8, 31, 2, 319, DateTimeKind.Local).AddTicks(6039), "123" }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "MajorId", "Term" },
                values: new object[,]
                {
                    { "IB0001", "IB", 4 },
                    { "IB0002", "IB", 5 },
                    { "IT0001", "IT", 2 },
                    { "IT0002", "IT", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_MajorId",
                table: "Student",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
