using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Werkgever.Migrations
{
    public partial class WerkgeverWhkPremiesTotaalAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Totaal",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                computedColumnSql: "[FlexWerkgever] + [FlexWerknemer] + [WgaVastWerkgever] + [WgaVastWerknemer] + [ZwFlex]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Totaal",
                table: "WerkgeverWhkPremies");
        }
    }
}
