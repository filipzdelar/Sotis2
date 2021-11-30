using Microsoft.EntityFrameworkCore.Migrations;

namespace Sotis2.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SubjectID",
                table: "Question",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectID",
                table: "Question");
        }
    }
}
