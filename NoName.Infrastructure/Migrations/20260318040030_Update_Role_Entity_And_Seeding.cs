using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Role_Entity_And_Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 11, 0, 29, 486, DateTimeKind.Local).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "16ac6299-c1ab-4895-9589-f438b7c6a724", "AQAAAAIAAYagAAAAEA1iwPF7BygFW6xSZ39qOqx9uX96TaoD+0s/buqn7THJ6s/H4yxntEGgalteUh86Jw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4ccf9361-16bc-4224-99c6-b87223226ea5"),
                column: "NormalizedName",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "ac69d33f-2a87-44db-8000-9cf5f3673ad7", "admin", "AQAAAAIAAYagAAAAEPp7qtXBJfJ4SZdmkR1La3QiAANS4IDJRKwN3u3AU3Dv6AMprkHhHQ7RVh9N5rSftw==" });
        }
    }
}
