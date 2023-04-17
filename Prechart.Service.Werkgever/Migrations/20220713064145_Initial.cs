using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Werkgever.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Werkgever",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlantId = table.Column<int>(type: "int", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sector = table.Column<int>(type: "int", nullable: false),
                    FiscaalNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoonheffingenExtentie = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    OmzetbelastingExtentie = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    DatumActiefVanaf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumActiefTot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Actief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werkgever", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WerkgeverWhkPremies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WerkgeverId = table.Column<int>(type: "int", nullable: false),
                    WgaVastWerkgever = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WgaVastWerknemer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlexWerkgever = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlexWerknemer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ZwFlex = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WgaFlexWerknemer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Totaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActiefVanaf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiefTot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Actief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WerkgeverWhkPremies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Werkgever_Id_Naam",
                table: "Werkgever",
                columns: new[] { "Id", "Naam" });

            migrationBuilder.CreateIndex(
                name: "IX_WerkgeverWhkPremies_Id_WerkgeverId",
                table: "WerkgeverWhkPremies",
                columns: new[] { "Id", "WerkgeverId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Werkgever");

            migrationBuilder.DropTable(
                name: "WerkgeverWhkPremies");
        }
    }
}
