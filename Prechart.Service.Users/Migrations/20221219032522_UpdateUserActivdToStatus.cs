using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prechart.Service.Users.Migrations
{
    public partial class UpdateUserActivdToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "Status" },
                values: new object[] { "b7b2aafe-c3c9-47ee-b8b3-98c38d79e88c", "AQAAAAEAACcQAAAAEAXCTOPy1MV/oDtdvryaehky6D0wMnUtX8nMHzsNXSHJwVi82PfK+P2VMIBUBcmfJg==", "1f94d1e5-e507-46d3-98fd-981b2216c673", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "Active", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { true, "15e6a20c-ae8d-4f80-8819-17eada6f70d2", "AQAAAAEAACcQAAAAEAcApZLaUmkTHBc+iOYHFoorVUwH/xHJfvnSDezqaUUvDKsXKXMliZHHFzdI/zaCqw==", "c17fb6c3-5b7e-4981-9df4-5cfe9d1ddf99" });
        }
    }
}
