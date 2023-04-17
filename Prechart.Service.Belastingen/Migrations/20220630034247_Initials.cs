using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Belastingen.Migrations
{
    public partial class Initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AOW",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeborenNa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeborenVoor = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GerechtigdIn = table.Column<int>(type: "int", nullable: true),
                    ExtraMaandenNa65 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AOW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Green",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Tabelloon = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    MetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderMetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderVerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterMetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterVerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Green", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Landen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandenCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LandenNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WoonlandbeginselIndicatie = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PremiePercentagesSocialeVerzekeringen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jaar = table.Column<int>(type: "int", nullable: false),
                    PremiePercentageAlgemeneOuderdomsWet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageNabestaanden = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageWetLangdurigeZorg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximaalSociaalverzekeringPremieloon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageAlgemeenWerkloosheidsFondsLaag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageAlgemeenWerkloosheidsFondsHoog = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageUitvoeringsdfondsvoordeOverheid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageWetArbeidsongeschikheidLaag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageWetArbeidsongeschikheidHoog = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageWetKinderopvang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageZiektekostenverzekeringWerkgeversbijdrage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiePercentageZiektekostenverzekeringWerknemersbijdrage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximaalZiektekostenverzekeringLoon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SocialeVerzekeringenRecordActief = table.Column<bool>(type: "bit", nullable: false),
                    SocialeVerzekeringenRecordActiefVanaf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialeVerzekeringenRecordActiefTot = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiePercentagesSocialeVerzekeringen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "White",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Tabelloon = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    MetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderMetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    EerderVerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterZonderLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterMetLoonheffingskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LaterVerrekendeArbeidskorting = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_White", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Woonlandbeginsel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WoonlandbeginselCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WoonlandbeginselBenaming = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WoonlandbeginselBelastingCode = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Woonlandbeginsel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AOW",
                columns: new[] { "Id", "ExtraMaandenNa65", "GeborenNa", "GeborenVoor", "GerechtigdIn" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1947, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1948, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2013 },
                    { 2, 2, new DateTime(1948, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1949, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2014 },
                    { 3, 3, new DateTime(1949, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1950, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2015 },
                    { 4, 6, new DateTime(1950, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1951, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2016 },
                    { 5, 9, new DateTime(1951, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1952, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2017 },
                    { 6, 12, new DateTime(1952, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1953, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2018 },
                    { 7, 16, new DateTime(1952, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1953, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2019 },
                    { 8, 16, new DateTime(1953, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1954, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2020 },
                    { 9, 16, new DateTime(1954, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1955, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2021 },
                    { 10, 19, new DateTime(1955, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1956, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2022 },
                    { 11, 22, new DateTime(1956, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1957, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2023 },
                    { 12, 24, new DateTime(1957, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1958, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2024 },
                    { 13, 24, new DateTime(1958, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2026 },
                    { 14, 24, new DateTime(1957, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1959, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2025 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 1, true, "AD", "Andorra", 2 },
                    { 2, true, "AE", "Verenigde Arabische Emiraten", 2 },
                    { 3, true, "AF", "Afghanistan", 2 },
                    { 4, true, "AG", "Antigua en Barbuda", 2 },
                    { 5, true, "AI", "Anguilla", 2 },
                    { 6, true, "AL", "Albanië", 2 },
                    { 7, true, "AM", "Armenië", 2 },
                    { 8, true, "AO", "Angola", 2 },
                    { 9, true, "AQ", "Antarctica", 2 },
                    { 10, true, "AR", "Argentinië", 2 },
                    { 11, true, "AS", "Amerikaans-Samoa", 2 },
                    { 12, true, "AT", "Oostenrijk", 1 },
                    { 13, true, "AU", "Australië", 2 },
                    { 14, true, "AW", "Aruba", 4 },
                    { 15, true, "AX", "Åland", 2 },
                    { 16, true, "AZ", "Azerbeidzjan", 2 },
                    { 17, true, "BA", "Bosnië en Herzegovina", 2 },
                    { 18, true, "BB", "Barbados", 2 },
                    { 19, true, "BD", "Bangladesh", 2 },
                    { 20, true, "BE", "België", 3 },
                    { 21, true, "BF", "Burkina Faso", 2 },
                    { 22, true, "BG", "Bulgarije", 1 },
                    { 23, true, "BH", "Bahrein", 2 },
                    { 24, true, "BI", "Burundi", 2 },
                    { 25, true, "BJ", "Benin", 2 },
                    { 26, true, "BL", "Saint-Barthélemy", 2 },
                    { 27, true, "BM", "Bermuda", 2 },
                    { 28, true, "BN", "Brunei", 2 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 29, true, "BO", "Bolivia", 2 },
                    { 30, true, "BQ", "Caribisch Nederland", 1 },
                    { 31, true, "BR", "Brazilië", 2 },
                    { 32, true, "BS", "Bahamas", 2 },
                    { 33, true, "BT", "Bhutan", 2 },
                    { 34, true, "BV", "Bouveteiland", 2 },
                    { 35, true, "BW", "Botswana", 2 },
                    { 36, true, "BY", "Wit-Rusland (Belarus)", 2 },
                    { 37, true, "BZ", "Belize", 2 },
                    { 38, true, "CA", "Canada", 2 },
                    { 39, true, "CC", "Cocoseilanden", 2 },
                    { 40, true, "CD", "Congo-Kinshasa", 2 },
                    { 41, true, "CF", "Centraal-Afrikaanse Republiek", 2 },
                    { 42, true, "CG", "Congo-Brazzaville", 2 },
                    { 43, true, "CH", "Zwitserland", 1 },
                    { 44, true, "CI", "Ivoorkust", 2 },
                    { 45, true, "CK", "Cookeilanden", 2 },
                    { 46, true, "CL", "Chili", 2 },
                    { 47, true, "CM", "Kameroen", 2 },
                    { 48, true, "CN", "China", 2 },
                    { 49, true, "CO", "Colombia", 2 },
                    { 50, true, "CR", "Costa Rica", 2 },
                    { 51, true, "CU", "Cuba", 2 },
                    { 52, true, "CV", "Kaapverdië", 2 },
                    { 53, true, "CW", "Curaçao", 2 },
                    { 54, true, "CX", "Christmaseiland", 2 },
                    { 55, true, "CY", "Cyprus", 1 },
                    { 56, true, "CZ", "Tsjechië", 1 },
                    { 57, true, "DE", "Duitsland", 1 },
                    { 58, true, "DJ", "Djibouti", 2 },
                    { 59, true, "DK", "Denemarken", 1 },
                    { 60, true, "DM", "Dominica", 2 },
                    { 61, true, "DO", "Dominicaanse Republiek", 2 },
                    { 62, true, "DZ", "Algerije", 2 },
                    { 63, true, "EC", "Ecuador", 2 },
                    { 64, true, "EE", "Estland", 1 },
                    { 65, true, "EG", "Egypte", 2 },
                    { 66, true, "EH", "Westelijke Sahara", 2 },
                    { 67, true, "ER", "Eritrea", 2 },
                    { 68, true, "ES", "Spanje", 1 },
                    { 69, true, "ET", "Ethiopië", 2 },
                    { 70, true, "FI", "Finland", 1 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 71, true, "FJ", "Fiji", 2 },
                    { 72, true, "FK", "Falklandeilanden", 2 },
                    { 73, true, "FM", "Micronesia", 2 },
                    { 74, true, "FO", "Faeröer", 2 },
                    { 75, true, "FR", "Frankrijk", 1 },
                    { 76, true, "GA", "Gabon", 2 },
                    { 77, true, "GB", "Verenigd Koninkrijk", 2 },
                    { 78, true, "GD", "Grenada", 2 },
                    { 79, true, "GE", "Georgië", 2 },
                    { 80, true, "GF", "Frans-Guyana", 2 },
                    { 81, true, "GG", "Guernsey", 2 },
                    { 82, true, "GH", "Ghana", 2 },
                    { 83, true, "GI", "Gibraltar", 2 },
                    { 84, true, "GL", "Groenland", 2 },
                    { 85, true, "GM", "Gambia", 2 },
                    { 86, true, "GN", "Guinee", 2 },
                    { 87, true, "GP", "Guadeloupe", 2 },
                    { 88, true, "GQ", "Equatoriaal-Guinea", 2 },
                    { 89, true, "GR", "Griekenland", 2 },
                    { 90, true, "GS", "Zuid-Georgia en de Zuidelijke Sandwicheilanden", 2 },
                    { 91, true, "GT", "Guatemala", 2 },
                    { 92, true, "GU", "Guam", 2 },
                    { 93, true, "GW", "Guinee-Bissau", 2 },
                    { 94, true, "GY", "Guyana", 2 },
                    { 95, true, "HK", "Hongkong", 2 },
                    { 96, true, "HM", "Heard en McDonaldeilanden", 2 },
                    { 97, true, "HN", "Honduras", 2 },
                    { 98, true, "HR", "Kroatië", 1 },
                    { 99, true, "HT", "Haïti", 2 },
                    { 100, true, "HU", "Hongarije", 1 },
                    { 101, true, "ID", "Indonesië", 2 },
                    { 102, true, "IE", "Ierland", 1 },
                    { 103, true, "IL", "Israël", 2 },
                    { 104, true, "IM", "Man (Eiland)", 2 },
                    { 105, true, "IN", "India", 2 },
                    { 106, true, "IO", "Brits Indische Oceaanterritorium", 2 },
                    { 107, true, "IQ", "Irak", 2 },
                    { 108, true, "IR", "Iran", 2 },
                    { 109, true, "IS", "IJsland", 1 },
                    { 110, true, "IT", "Italië", 1 },
                    { 111, true, "JE", "Jersey", 2 },
                    { 112, true, "JM", "Jamaica", 2 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 113, true, "JO", "Jordanië", 2 },
                    { 114, true, "JP", "Japan", 2 },
                    { 115, true, "KE", "Kenia", 2 },
                    { 116, true, "KG", "Kirgizië", 2 },
                    { 117, true, "KH", "Cambodja", 2 },
                    { 118, true, "KI", "Kiribati", 2 },
                    { 119, true, "KM", "Comoren", 2 },
                    { 120, true, "KN", "Saint Kitts en Nevis", 2 },
                    { 121, true, "KP", "Noord-Korea", 2 },
                    { 122, true, "KR", "Zuid-Korea", 2 },
                    { 123, true, "KW", "Koeweit", 2 },
                    { 124, true, "KY", "Caymaneilanden", 2 },
                    { 125, true, "KZ", "Kazachstan", 2 },
                    { 126, true, "LA", "Laos", 2 },
                    { 127, true, "LB", "Libanon", 2 },
                    { 128, true, "LC", "Saint Lucia", 2 },
                    { 129, true, "LI", "Liechtenstein", 1 },
                    { 130, true, "LK", "Sri Lanka", 2 },
                    { 131, true, "LR", "Liberia", 2 },
                    { 132, true, "LS", "Lesotho", 2 },
                    { 133, true, "LT", "Litouwen", 1 },
                    { 134, true, "LU", "Luxemburg", 1 },
                    { 135, true, "LV", "Letland", 1 },
                    { 136, true, "LY", "Libië", 2 },
                    { 137, true, "MA", "Marokko", 2 },
                    { 138, true, "MC", "Monaco", 2 },
                    { 139, true, "MD", "Moldavië", 2 },
                    { 140, true, "ME", "Montenegro", 2 },
                    { 141, true, "MF", "Sint Maarten (Frans deel)", 2 },
                    { 142, true, "MG", "Madagaskar", 2 },
                    { 143, true, "MH", "Marshalleilanden", 2 },
                    { 144, true, "MK", "Noord-Macedonië", 2 },
                    { 145, true, "ML", "Mali", 2 },
                    { 146, true, "MM", "Myanmar", 2 },
                    { 147, true, "MN", "Mongolië", 2 },
                    { 148, true, "MO", "Macau", 2 },
                    { 149, true, "MP", "Noordelijke Marianen", 2 },
                    { 150, true, "MQ", "Martinique", 2 },
                    { 151, true, "MR", "Mauritanië", 2 },
                    { 152, true, "MS", "Montserrat", 2 },
                    { 153, true, "MT", "Malta", 1 },
                    { 154, true, "MU", "Mauritius", 2 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 155, true, "MV", "Malediven", 2 },
                    { 156, true, "MW", "Malawi", 2 },
                    { 157, true, "MX", "Mexico", 2 },
                    { 158, true, "MY", "Maleisië", 2 },
                    { 159, true, "MZ", "Mozambique", 2 },
                    { 160, true, "NA", "Namibië", 2 },
                    { 161, true, "NC", "Nieuw-Caledonië", 2 },
                    { 162, true, "NE", "Niger", 2 },
                    { 163, true, "NF", "Norfolk", 2 },
                    { 164, true, "NG", "Nigeria", 2 },
                    { 165, true, "NI", "Nicaragua", 2 },
                    { 166, true, "NL", "Nederland", 0 },
                    { 167, true, "NO", "Noorwegen", 1 },
                    { 168, true, "NP", "Nepal", 2 },
                    { 169, true, "NR", "Nauru", 2 },
                    { 170, true, "NU", "Niue", 2 },
                    { 171, true, "NZ", "Nieuw-Zeeland", 2 },
                    { 172, true, "OM", "Oman", 2 },
                    { 173, true, "PA", "Panama", 2 },
                    { 174, true, "PE", "Peru", 2 },
                    { 175, true, "PF", "Frans-Polynesië", 2 },
                    { 176, true, "PG", "Papoea-Nieuw-Guinea", 2 },
                    { 177, true, "PH", "Filipijnen", 2 },
                    { 178, true, "PK", "Pakistan", 2 },
                    { 179, true, "PL", "Polen", 1 },
                    { 180, true, "PM", "Saint-Pierre en Miquelon", 2 },
                    { 181, true, "PN", "Pitcairneilanden", 2 },
                    { 182, true, "PR", "Puerto Rico", 2 },
                    { 183, true, "PS", "Palestina", 2 },
                    { 184, true, "PT", "Portugal", 1 },
                    { 185, true, "PW", "Palau", 2 },
                    { 186, true, "PY", "Paraguay", 2 },
                    { 187, true, "QA", "Qatar", 2 },
                    { 188, true, "RE", "Réunion", 2 },
                    { 189, true, "RO", "Roemenië", 1 },
                    { 190, true, "RS", "Servië", 2 },
                    { 191, true, "RU", "Rusland", 2 },
                    { 192, true, "RW", "Rwanda", 2 },
                    { 193, true, "SA", "Saoedi-Arabië", 2 },
                    { 194, true, "SB", "Salomonseilanden", 2 },
                    { 195, true, "SC", "Seychellen", 2 },
                    { 196, true, "SD", "Sudan", 2 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 197, true, "SE", "Zweden", 1 },
                    { 198, true, "SG", "Singapore", 2 },
                    { 199, true, "SH", "Sint-Helena, Ascension en Tristan da Cunha", 2 },
                    { 200, true, "SI", "Slovenië", 1 },
                    { 201, true, "SJ", "Spitsbergen en Jan Mayen", 2 },
                    { 202, true, "SK", "Slowakije", 1 },
                    { 203, true, "SL", "Sierra Leone", 2 },
                    { 204, true, "SM", "San Marino", 2 },
                    { 205, true, "SN", "Senegal", 2 },
                    { 206, true, "SO", "Somalië", 2 },
                    { 207, true, "SR", "Suriname", 4 },
                    { 208, true, "SS", "Zuid-Sudan", 2 },
                    { 209, true, "ST", "Sao Tomé en Principe", 2 },
                    { 210, true, "SV", "El Salvador", 2 },
                    { 211, true, "SX", "Sint Maarten (Nederlands deel)", 2 },
                    { 212, true, "SY", "Syrië", 2 },
                    { 213, true, "SZ", "Swaziland", 2 },
                    { 214, true, "TC", "Turks- en Caicoseilanden", 2 },
                    { 215, true, "TD", "Tsjaad", 2 },
                    { 216, true, "TF", "Franse Zuidelijke en Antarctische Gebieden", 2 },
                    { 217, true, "TG", "Togo", 2 },
                    { 218, true, "TH", "Thailand", 2 },
                    { 219, true, "TJ", "Tadzjikistan", 2 },
                    { 220, true, "TK", "Tokelau", 2 },
                    { 221, true, "TL", "Oost-Timor", 2 },
                    { 222, true, "TM", "Turkmenistan", 2 },
                    { 223, true, "TN", "Tunesië", 2 },
                    { 224, true, "TO", "Tonga", 2 },
                    { 225, true, "TR", "Turkije", 2 },
                    { 226, true, "TT", "Trinidad en Tobago", 2 },
                    { 227, true, "TV", "Tuvalu", 2 },
                    { 228, true, "TW", "Taiwan", 2 },
                    { 229, true, "TZ", "Tanzania", 2 },
                    { 230, true, "UA", "Oekraïne", 2 },
                    { 231, true, "UG", "Uganda", 2 },
                    { 232, true, "UM", "Kleine afgelegen eilanden van de Verenigde Staten", 2 },
                    { 233, true, "US", "Verenigde Staten", 2 },
                    { 234, true, "UY", "Uruguay", 2 },
                    { 235, true, "UZ", "Oezbekistan", 2 },
                    { 236, true, "VA", "Vaticaanstad (de Heilige Stoel)", 2 },
                    { 237, true, "VC", "Saint Vincent en de Grenadines", 2 },
                    { 238, true, "VE", "Venezuela", 2 }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "Id", "Active", "LandenCode", "LandenNaam", "WoonlandbeginselIndicatie" },
                values: new object[,]
                {
                    { 239, true, "VG", "Britse Maagdeneilanden", 2 },
                    { 240, true, "VI", "Amerikaanse Maagdeneilanden", 2 },
                    { 241, true, "VN", "Vietnam", 2 },
                    { 242, true, "VU", "Vanuatu", 2 },
                    { 243, true, "WF", "Wallis en Futuna", 2 },
                    { 244, true, "WS", "Samoa", 2 },
                    { 245, true, "YE", "Jemen", 2 },
                    { 246, true, "YT", "Mayotte", 2 },
                    { 247, true, "ZA", "Zuid-Afrika", 2 },
                    { 248, true, "ZM", "Zambia", 2 },
                    { 249, true, "ZW", "Zimbabwe", 2 }
                });

            migrationBuilder.InsertData(
                table: "PremiePercentagesSocialeVerzekeringen",
                columns: new[] { "Id", "Jaar", "MaximaalSociaalverzekeringPremieloon", "MaximaalZiektekostenverzekeringLoon", "PremiePercentageAlgemeenWerkloosheidsFondsHoog", "PremiePercentageAlgemeenWerkloosheidsFondsLaag", "PremiePercentageAlgemeneOuderdomsWet", "PremiePercentageNabestaanden", "PremiePercentageUitvoeringsdfondsvoordeOverheid", "PremiePercentageWetArbeidsongeschikheidHoog", "PremiePercentageWetArbeidsongeschikheidLaag", "PremiePercentageWetKinderopvang", "PremiePercentageWetLangdurigeZorg", "PremiePercentageZiektekostenverzekeringWerkgeversbijdrage", "PremiePercentageZiektekostenverzekeringWerknemersbijdrage", "SocialeVerzekeringenRecordActief", "SocialeVerzekeringenRecordActiefTot", "SocialeVerzekeringenRecordActiefVanaf" },
                values: new object[] { 1, 2022, 59706.00m, 59706.00m, 7.7m, 2.7m, 17.90m, 0.10m, 0.68m, 7.05m, 5.49m, 0.5m, 9.65m, 6.75m, 5.50m, true, new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Woonlandbeginsel",
                columns: new[] { "Id", "Active", "WoonlandbeginselBelastingCode", "WoonlandbeginselBenaming", "WoonlandbeginselCode" },
                values: new object[,]
                {
                    { 1, true, 2, "Netherlands", "NL" },
                    { 2, true, 2, "Belgium", "BE" },
                    { 3, true, 2, "Landenkring", "LK" },
                    { 4, true, 2, "DerdeLanden", "DL" },
                    { 5, true, 2, "SurinameAruba", "SA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Green_Id_Year_CountryId_TypeId",
                table: "Green",
                columns: new[] { "Id", "Year", "CountryId", "TypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Landen_Id_LandenCode",
                table: "Landen",
                columns: new[] { "Id", "LandenCode" });

            migrationBuilder.CreateIndex(
                name: "IX_White_Id_Year_CountryId_TypeId",
                table: "White",
                columns: new[] { "Id", "Year", "CountryId", "TypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Woonlandbeginsel_Id_WoonlandbeginselCode",
                table: "Woonlandbeginsel",
                columns: new[] { "Id", "WoonlandbeginselCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AOW");

            migrationBuilder.DropTable(
                name: "Green");

            migrationBuilder.DropTable(
                name: "Landen");

            migrationBuilder.DropTable(
                name: "PremiePercentagesSocialeVerzekeringen");

            migrationBuilder.DropTable(
                name: "White");

            migrationBuilder.DropTable(
                name: "Woonlandbeginsel");
        }
    }
}
