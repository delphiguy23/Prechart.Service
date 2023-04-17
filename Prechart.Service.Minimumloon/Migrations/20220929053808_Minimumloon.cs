using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Minimumloon.Migrations
{
    public partial class Minimumloon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Minimumloon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jaar = table.Column<int>(type: "int", nullable: false),
                    MinimumloonLeeftijd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerMaand = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerWeek = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerDag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerUur36 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerUur38 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonPerUur40 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumloonRecordActiefVanaf = table.Column<DateTime>(type: "DateTime", nullable: false),
                    MinimumloonRecordActiefTot = table.Column<DateTime>(type: "DateTime", nullable: false),
                    MinimumloonRecordActief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Minimumloon", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Minimumloon_Id",
                table: "Minimumloon",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Minimumloon");
        }
    }
}
