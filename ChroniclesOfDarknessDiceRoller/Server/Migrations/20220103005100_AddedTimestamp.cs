using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChroniclesOfDarknessDiceRoller.Server.Migrations
{
    public partial class AddedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Timestamp",
                table: "DiceRolls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "DiceRolls");
        }
    }
}
