using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteCNPQ.Migrations
{
    public partial class Migration_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalNome",
                table: "Agendamento",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalNome",
                table: "Agendamento");
        }
    }
}
