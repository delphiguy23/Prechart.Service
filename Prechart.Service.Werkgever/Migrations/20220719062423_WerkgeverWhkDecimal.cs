using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Werkgever.Migrations
{
    public partial class WerkgeverWhkDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WgaFlexWerknemer",
                table: "WerkgeverWhkPremies");

            migrationBuilder.AlterColumn<decimal>(
                name: "ZwFlex",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "OmzetbelastingExtentie",
                table: "Werkgever",
                type: "nvarchar(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Werkgever",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoonheffingenExtentie",
                table: "Werkgever",
                type: "nvarchar(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)");

            migrationBuilder.AlterColumn<string>(
                name: "FiscaalNummer",
                table: "Werkgever",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Totaal",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,3)",
                nullable: false,
                computedColumnSql: "[FlexWerkgever] + [FlexWerknemer] + [WgaVastWerkgever] + [WgaVastWerknemer] + [ZwFlex]",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ZwFlex",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WgaVastWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Totaal",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldComputedColumnSql: "[FlexWerkgever] + [FlexWerknemer] + [WgaVastWerkgever] + [WgaVastWerknemer] + [ZwFlex]");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlexWerkgever",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AddColumn<decimal>(
                name: "WgaFlexWerknemer",
                table: "WerkgeverWhkPremies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "OmzetbelastingExtentie",
                table: "Werkgever",
                type: "nvarchar(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Werkgever",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoonheffingenExtentie",
                table: "Werkgever",
                type: "nvarchar(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FiscaalNummer",
                table: "Werkgever",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
