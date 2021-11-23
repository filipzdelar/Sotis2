using Microsoft.EntityFrameworkCore.Migrations;

namespace Sotis2.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswareText",
                table: "Answare",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItTrue",
                table: "Answare",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswareText",
                table: "Answare");

            migrationBuilder.DropColumn(
                name: "IsItTrue",
                table: "Answare");
        }
    }
}
