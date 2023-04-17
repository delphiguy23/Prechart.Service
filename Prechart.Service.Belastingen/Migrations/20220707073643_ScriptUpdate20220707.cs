using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class ScriptUpdate20220707 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 1,
                column: "WoonlandbeginselBenaming",
                value: "Nederlands");

            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 2,
                column: "WoonlandbeginselBenaming",
                value: "België");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 1,
                column: "WoonlandbeginselBenaming",
                value: "Netherlands");

            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 2,
                column: "WoonlandbeginselBenaming",
                value: "Belgium");
        }
    }
}
