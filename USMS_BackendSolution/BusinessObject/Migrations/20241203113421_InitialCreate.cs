using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    { 2, "Academic Staff" },
                    { 3, "Chairperson" },
                    { 4, "Lecture" },
                    { 5, "Student" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "DateOfBirth", "Email", "FirstName", "LastName", "MajorId", "MiddleName", "PasswordHash", "PersonalEmail", "PhoneNumber", "RoleId", "Status", "UpdatedAt", "UserAvartar" },
                values: new object[,]
                {
                    { "AD0001", new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7142), new DateOnly(2000, 9, 30), "admin0001@university.edu", "Nguyen", "Admin", null, "Tuan", "hashedpassword3", "admin0001@gmail.edu", "0123456780", 1, 1, new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7142), "123" },
                    { "IB0001", new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7137), new DateOnly(2002, 9, 30), "BTTIB0001.b@university.edu", "Tran", "B", "IB", "Thi", "hashedpassword2", "thi.b@gmail.com", "0987654321", 5, 1, new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7138), "123" },
                    { "IT0001", new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7124), new DateOnly(2002, 4, 30), "ANVIT0001.a@university.edu", "Nguyen", "A", "IT", "Van", "hashedpassword1", "van.a@gmail.com", "0123456789", 5, 1, new DateTime(2024, 12, 3, 18, 34, 21, 159, DateTimeKind.Local).AddTicks(7134), "123" }
                });

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
                name: "Customer");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
