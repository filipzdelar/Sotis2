using Microsoft.EntityFrameworkCore.Migrations;

namespace Sotis2.Migrations
{
    public partial class Init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOfSubject",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOfSubject",
                table: "Subject");
        }
    }
}
