using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChroniclesOfDarknessDiceRoller.Server.Migrations
{
    public partial class RemoveKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls",
                column: "GameName");
        }
    }
}
