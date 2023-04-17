using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class UpdateWoonland_20220708 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 1,
                column: "WoonlandbeginselBenaming",
                value: "Nederland");

            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 4,
                column: "WoonlandbeginselBenaming",
                value: "Derde Landen");

            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 5,
                column: "WoonlandbeginselBenaming",
                value: "Suriname/Aruba");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: 4,
                column: "WoonlandbeginselBenaming",
                value: "DerdeLanden");

            migrationBuilder.UpdateData(
                table: "Woonlandbeginsel",
                keyColumn: "Id",
                keyValue: 5,
                column: "WoonlandbeginselBenaming",
                value: "SurinameAruba");
        }
    }
}
