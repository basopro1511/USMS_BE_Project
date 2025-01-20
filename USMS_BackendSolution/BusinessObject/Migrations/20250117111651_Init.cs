using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", nullable: false),
                    MajorName = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
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
                    RoleName = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "NVARCHAR(8)", nullable: false),
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", nullable: true),
                    Term = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "MajorId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR(8)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    MiddleName = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    LastName = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PersonalEmail = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    UserAvartar = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MajorId = table.Column<string>(type: "NVARCHAR(4)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "MajorId");
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Major",
                columns: new[] { "MajorId", "MajorName" },
                values: new object[,]
                {
                    { "BA", "Business Administration" },
                    { "IT", "Information Technology" },
                    { "MT", "Media Technology" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Academic Staff" },
                    { 3, "Chairperson" },
                    { 4, "Teacher" },
                    { 5, "Student" }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "MajorId", "Term" },
                values: new object[] { "CE170288", "IT", 1 });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "DateOfBirth", "Email", "FirstName", "LastName", "MajorId", "MiddleName", "PasswordHash", "PersonalEmail", "PhoneNumber", "RoleId", "Status", "UpdatedAt", "UserAvartar" },
                values: new object[] { "CE170288", new DateTime(2025, 1, 17, 18, 16, 50, 778, DateTimeKind.Local).AddTicks(3412), new DateOnly(2025, 11, 15), "HoangNQCE170288@fpt.edu.vn", "Nguyễn", "Hoàng", "IT", "Quốc", "123456", "Hoang@gmail.com", "0333744591", 1, 1, new DateTime(2025, 1, 17, 18, 16, 50, 778, DateTimeKind.Local).AddTicks(3422), null });

            migrationBuilder.CreateIndex(
                name: "IX_Student_MajorId",
                table: "Student",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MajorId",
                table: "User",
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
                name: "Student");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
