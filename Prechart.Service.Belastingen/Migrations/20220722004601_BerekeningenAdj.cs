using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class BerekeningenAdj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasisDagen",
                table: "Berekeningen",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Payee",
                table: "Berekeningen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PremieBedragZiektekostenVerzekeringsWetWerkgeversOrWerknemer",
                table: "Berekeningen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasisDagen",
                table: "Berekeningen");

            migrationBuilder.DropColumn(
                name: "Payee",
                table: "Berekeningen");

            migrationBuilder.DropColumn(
                name: "PremieBedragZiektekostenVerzekeringsWetWerkgeversOrWerknemer",
                table: "Berekeningen");
        }
    }
}
