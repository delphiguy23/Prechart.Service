using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Werkgever.Migrations
{
    public partial class WerkgeverWhkPremiesTotaalDrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Totaal",
                table: "WerkgeverWhkPremies");

            migrationBuilder.AlterColumn<decimal>(
                name: "ZwFlex",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ZwFlex",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "Totaal",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                computedColumnSql: "[FlexWerkgever] + [FlexWerknemer] + [WgaVastWerkgever] + [WgaVastWerknemer] + [ZwFlex]");
        }
    }
}
