using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChroniclesOfDarknessDiceRoller.Server.Migrations
{
    public partial class AddId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DiceRolls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DiceRolls");
        }
    }
}
