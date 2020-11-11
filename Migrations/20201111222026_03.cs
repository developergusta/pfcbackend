using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticket2U.API.Migrations
{
    public partial class _03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LotCategoryId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LotCategoryId",
                table: "Tickets",
                column: "LotCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_LotCategories_LotCategoryId",
                table: "Tickets",
                column: "LotCategoryId",
                principalTable: "LotCategories",
                principalColumn: "LotCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_LotCategories_LotCategoryId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LotCategoryId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LotCategoryId",
                table: "Tickets");
        }
    }
}
