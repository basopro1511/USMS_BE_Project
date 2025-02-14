using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    { "AD0001", "FPT Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3759), new DateOnly(2005, 11, 15), "Admin@fpt.edu.vn", "Nguyễn", "Hoàng", "SE", "Quốc", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Admin@gmail.com", "0333744591", 1, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3768), null },
                    { "BA0003", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3779), new DateOnly(2003, 12, 5), "NhungLHBA170290@fpt.edu.vn", "Lê", "Nhung", "BA", "Hồng", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Nhung@gmail.com", "0987654321", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3779), null },
                    { "CT0005", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3784), new DateOnly(2003, 7, 30), "DungDVCT170292@fpt.edu.vn", "Đinh", "Dũng", "CT", "Văn", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Dung@gmail.com", "0966998844", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3785), null },
                    { "LG0004", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3781), new DateOnly(2002, 5, 20), "ChauPMLG170291@fpt.edu.vn", "Phạm", "Châu", "LG", "Minh", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Chau@gmail.com", "0978111222", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3782), null },
                    { "SE0001", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3772), new DateOnly(2005, 11, 15), "HoangNQCE170288@fpt.edu.vn", "Nguyễn", "Hoàng", "SE", "Quốc", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Hoang@gmail.com", "0333744591", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3772), null },
                    { "SE0002", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3776), new DateOnly(2004, 9, 10), "TungTTCE170289@fpt.edu.vn", "Trần", "Tùng", "SE", "Thanh", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Tung@gmail.com", "0322114477", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3776), null },
                    { "SE0006", "Can Tho", new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3787), new DateOnly(2001, 8, 25), "CuongVMIT170293@fpt.edu.vn", "Vũ", "Cường", "SE", "Mạnh", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "Cuong@gmail.com", "0356677889", 5, 1, new DateTime(2025, 2, 13, 16, 58, 10, 383, DateTimeKind.Local).AddTicks(3787), null }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "MajorId", "Term" },
                values: new object[,]
                {
                    { "BA0003", "BA", 2 },
                    { "CT0005", "CT", 4 },
                    { "LG0004", "LG", 3 },
                    { "SE0001", "SE", 1 },
                    { "SE0002", "SE", 1 },
                    { "SE0006", "SE", 5 }
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
