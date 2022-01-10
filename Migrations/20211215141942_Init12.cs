using Microsoft.EntityFrameworkCore.Migrations;

namespace Sotis2.Migrations
{
    public partial class Init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EdgeDD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainFromID = table.Column<long>(type: "bigint", nullable: false),
                    DomainToID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeDD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EdgeQD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionFromID = table.Column<long>(type: "bigint", nullable: false),
                    DomainToID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeQD", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EdgeDD");

            migrationBuilder.DropTable(
                name: "EdgeQD");
        }
    }
}
