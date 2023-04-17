using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Users.Migrations
{
    public partial class ApiKeyAndRefreshTokenUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ApiToken", "ConcurrencyStamp", "PasswordHash", "RefreshToken", "SecurityStamp" },
                values: new object[] { "JH+C1Fnv72VIXbmM8aS8+UXJ6ci8Bgtn5R1MeOksvdWz11qmVKNvVQrSsbYivtzBkBikwz6s3ycyY4nyf34i/Q==", "15e6a20c-ae8d-4f80-8819-17eada6f70d2", "AQAAAAEAACcQAAAAEAcApZLaUmkTHBc+iOYHFoorVUwH/xHJfvnSDezqaUUvDKsXKXMliZHHFzdI/zaCqw==", "dMQa7YJBXc0rgNQeBeeJnabu+mpChoi4NAkO+1WnhqS+A+fRESDU2svYGdWPTH+1OkpzeHeVBPw8TbJ9p/LKXg==", "c17fb6c3-5b7e-4981-9df4-5cfe9d1ddf99" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ApiToken", "ConcurrencyStamp", "PasswordHash", "RefreshToken", "SecurityStamp" },
                values: new object[] { null, "e432d04b-706f-438e-9692-04b36facb82d", "AQAAAAEAACcQAAAAEMgu30q44DInjINfvbFX3JS6LWqdPCExhUGe1h31CqzWA03h4UN6QfRF6th1msfmXg==", null, "ea4e6c0d-ac3a-45ed-8ac9-ccf1ec2f027d" });
        }
    }
}
