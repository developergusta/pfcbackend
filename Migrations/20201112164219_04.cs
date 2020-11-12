﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ticket2U.API.Migrations
{
    public partial class _04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashbackId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cashback",
                columns: table => new
                {
                    CashbackId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateSolicitation = table.Column<DateTime>(nullable: false),
                    DateCashback = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashback", x => x.CashbackId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CashbackId",
                table: "Tickets",
                column: "CashbackId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cashback_CashbackId",
                table: "Tickets",
                column: "CashbackId",
                principalTable: "Cashback",
                principalColumn: "CashbackId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cashback_CashbackId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Cashback");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CashbackId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CashbackId",
                table: "Tickets");
        }
    }
}
