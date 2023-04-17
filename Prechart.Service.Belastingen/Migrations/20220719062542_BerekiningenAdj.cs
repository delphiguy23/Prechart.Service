using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class BerekiningenAdj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groen",
                table: "Berekeningen");

            migrationBuilder.RenameColumn(
                name: "InhoudingopLoonWit",
                table: "Berekeningen",
                newName: "InhoudingOpLoonWit");

            migrationBuilder.RenameColumn(
                name: "InhoudingopLoonGroen",
                table: "Berekeningen",
                newName: "InhoudingOpLoonGroen");

            migrationBuilder.RenameColumn(
                name: "Wit",
                table: "Berekeningen",
                newName: "NettoTeBetalenSubTotaal");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Berekeningen",
                newName: "WoonlandbeginselId");

            migrationBuilder.RenameColumn(
                name: "NettoTeBetalen",
                table: "Berekeningen",
                newName: "NettoTeBetalenEindTotaal");

            migrationBuilder.RenameColumn(
                name: "LoonWit",
                table: "Berekeningen",
                newName: "InkomenWit");

            migrationBuilder.RenameColumn(
                name: "LoonGroen",
                table: "Berekeningen",
                newName: "InkomenGroen");

            migrationBuilder.RenameColumn(
                name: "Jaar",
                table: "Berekeningen",
                newName: "TijdvakId");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Berekeningen",
                newName: "PremieBedragWetArbeIdsOngeschikheIdLaagHoog");

            migrationBuilder.AddColumn<decimal>(
                name: "IsPremieBedragUitvoeringsFondsvoordeOverheId",
                table: "Berekeningen",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog",
                table: "Berekeningen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessDatum",
                table: "Berekeningen",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremieBedragUitvoeringsFondsvoordeOverheId",
                table: "Berekeningen");

            migrationBuilder.DropColumn(
                name: "PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog",
                table: "Berekeningen");

            migrationBuilder.DropColumn(
                name: "ProcessDatum",
                table: "Berekeningen");

            migrationBuilder.RenameColumn(
                name: "InhoudingOpLoonWit",
                table: "Berekeningen",
                newName: "InhoudingopLoonWit");

            migrationBuilder.RenameColumn(
                name: "InhoudingOpLoonGroen",
                table: "Berekeningen",
                newName: "InhoudingopLoonGroen");

            migrationBuilder.RenameColumn(
                name: "WoonlandbeginselId",
                table: "Berekeningen",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "TijdvakId",
                table: "Berekeningen",
                newName: "Jaar");

            migrationBuilder.RenameColumn(
                name: "PremieBedragWetArbeIdsOngeschikheIdLaagHoog",
                table: "Berekeningen",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "NettoTeBetalenSubTotaal",
                table: "Berekeningen",
                newName: "Wit");

            migrationBuilder.RenameColumn(
                name: "NettoTeBetalenEindTotaal",
                table: "Berekeningen",
                newName: "NettoTeBetalen");

            migrationBuilder.RenameColumn(
                name: "InkomenWit",
                table: "Berekeningen",
                newName: "LoonWit");

            migrationBuilder.RenameColumn(
                name: "InkomenGroen",
                table: "Berekeningen",
                newName: "LoonGroen");

            migrationBuilder.AddColumn<decimal>(
                name: "Groen",
                table: "Berekeningen",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
