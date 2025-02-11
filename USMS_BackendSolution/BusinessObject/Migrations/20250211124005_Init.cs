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
                    Address = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Student_User_StudentId",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Major",
                columns: new[] { "MajorId", "MajorName" },
                values: new object[,]
                {
                    { "BA", "Quản trị kinh doanh" },
                    { "CT", "Công nghệ truyền thông" },
                    { "LG", "Ngôn ngữ" },
                    { "SE", "Kỹ thuật phần mềm" }
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
                table: "User",
                columns: new[] { "UserId", "Address", "CreatedAt", "DateOfBirth", "Email", "FirstName", "LastName", "MajorId", "MiddleName", "PasswordHash", "PersonalEmail", "PhoneNumber", "RoleId", "Status", "UpdatedAt", "UserAvartar" },
                values: new object[,]
                {
                    { "BA170290", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2425), new DateOnly(2003, 12, 5), "NhungLHBA170290@fpt.edu.vn", "Lê", "Nhung", "BA", "Hồng", "987654", "Nhung@gmail.com", "0987654321", 5, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2426), null },
                    { "CE170288", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2408), new DateOnly(2005, 11, 15), "HoangNQCE170288@fpt.edu.vn", "Nguyễn", "Hoàng", "SE", "Quốc", "123456", "Hoang@gmail.com", "0333744591", 1, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2417), null },
                    { "CE170289", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2422), new DateOnly(2004, 9, 10), "TungTTCE170289@fpt.edu.vn", "Trần", "Tùng", "SE", "Thanh", "654321", "Tung@gmail.com", "0322114477", 5, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2422), null },
                    { "CT170292", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2432), new DateOnly(2003, 7, 30), "DungDVCT170292@fpt.edu.vn", "Đinh", "Dũng", "CT", "Văn", "abcdef", "Dung@gmail.com", "0966998844", 5, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2433), null },
                    { "IT170293", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2435), new DateOnly(2001, 8, 25), "CuongVMIT170293@fpt.edu.vn", "Vũ", "Cường", "SE", "Mạnh", "password", "Cuong@gmail.com", "0356677889", 5, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2436), null },
                    { "LG170291", "Can Tho", new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2429), new DateOnly(2002, 5, 20), "ChauPMLG170291@fpt.edu.vn", "Phạm", "Châu", "LG", "Minh", "112233", "Chau@gmail.com", "0978111222", 5, 1, new DateTime(2025, 2, 11, 19, 40, 5, 422, DateTimeKind.Local).AddTicks(2430), null }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "MajorId", "Term" },
                values: new object[,]
                {
                    { "BA170290", "BA", 2 },
                    { "CE170288", "SE", 1 },
                    { "CE170289", "SE", 1 },
                    { "CT170292", "CT", 1 },
                    { "IT170293", "SE", 4 },
                    { "LG170291", "LG", 3 }
                });

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
