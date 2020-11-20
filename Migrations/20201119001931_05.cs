using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticket2U.API.Migrations
{
    public partial class _05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cashbacks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cashbacks");
        }
    }
}
