using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticket2U.API.Migrations
{
    public partial class _06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Lots",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Events",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
