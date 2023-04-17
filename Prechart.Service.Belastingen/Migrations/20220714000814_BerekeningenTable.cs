using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class BerekeningenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Berekeningen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jaar = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Wit = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Groen = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    KlantId = table.Column<int>(type: "int", nullable: false),
                    WerkgeverId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LoonOverVanaf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoonOverTot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoonInVanaf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoonInTot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoonWit = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LoonGroen = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AlgemeneHeffingskortingToegepast = table.Column<int>(type: "int", nullable: false),
                    InhoudingopLoonWit = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    InhoudingopLoonGroen = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AlgemeneHeffingskortingBedrag = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VerrekendeArbeIdskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    SociaalVerzekeringsloon = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragAlgemeenWerkloosheIdsFondsLaag = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragAlgemeenWerkloosheIdsFondsHoog = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragUitvoeringsFondsvoordeOverheId = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragWetArbeIdsOngeschikheIdLaag = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragWetArbeIdsOngeschikheIdHoog = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragWetKinderopvang = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragZiektekostenVerzekeringsWetLoon = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragWGAVastWerkgever = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragWGAVastWerknemer = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragFlexWerkgever = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragFlexWerknemer = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragZWFlex = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WerkgeverWHKPremieBedragTotaal = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NettoTeBetalen = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Actief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Berekeningen", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Berekeningen");
        }
    }
}
