using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteCNPQ.Migrations
{
    public partial class Migration_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocalId",
                table: "Agendamento",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Agendamento_LocalId",
                table: "Agendamento",
                column: "LocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamento_Local_LocalId",
                table: "Agendamento",
                column: "LocalId",
                principalTable: "Local",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamento_Local_LocalId",
                table: "Agendamento");

            migrationBuilder.DropIndex(
                name: "IX_Agendamento_LocalId",
                table: "Agendamento");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Agendamento");
        }
    }
}
