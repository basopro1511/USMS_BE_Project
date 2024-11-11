using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "Name", "Phone" },
                values: new object[] { "CE170288", "Phong Điền, Cần Thơ", "Nguyễn Quốcc Hoàng", "0333744591" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
