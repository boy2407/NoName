using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeRefreshTokenExpiryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2488));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2498));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2501));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 22, 22, 11, 990, DateTimeKind.Local).AddTicks(2502));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RefreshTokenExpiryTime" },
                values: new object[] { "ac69d33f-2a87-44db-8000-9cf5f3673ad7", "AQAAAAIAAYagAAAAEPp7qtXBJfJ4SZdmkR1La3QiAANS4IDJRKwN3u3AU3Dv6AMprkHhHQ7RVh9N5rSftw==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3705));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3718));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 17, 10, 55, 41, 795, DateTimeKind.Local).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RefreshTokenExpiryTime" },
                values: new object[] { "4ab13832-a837-4667-a036-e613176d6211", "AQAAAAIAAYagAAAAEFjk5pkCdoOsnYdTjs64YsM66vCBf2yxpbIJawCY7NtHs4JBdQfHO/c55HLAcN0dfA==", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
