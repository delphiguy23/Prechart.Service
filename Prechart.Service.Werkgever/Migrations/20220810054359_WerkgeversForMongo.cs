using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Werkgever.Migrations
{
    public partial class WerkgeversForMongo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Werkgever");

            migrationBuilder.AddColumn<string>(
                name: "WerkgeverWhkMongoId",
                table: "WerkgeverWhkPremies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KlantMongoId",
                table: "Werkgever",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WerkgeverMongoId",
                table: "Werkgever",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WerkgeverWhkMongoId",
                table: "WerkgeverWhkPremies");

            migrationBuilder.DropColumn(
                name: "KlantMongoId",
                table: "Werkgever");

            migrationBuilder.DropColumn(
                name: "WerkgeverMongoId",
                table: "Werkgever");

            migrationBuilder.AddColumn<int>(
                name: "KlantId",
                table: "Werkgever",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
