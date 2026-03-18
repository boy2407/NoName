using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryParentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2026, 3, 12, 21, 55, 23, 279, DateTimeKind.Local).AddTicks(969));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bd3259dd-c167-4f7b-8b66-970c76da6582", "AQAAAAIAAYagAAAAEMosH59WeiYLezon/BM8zOOyY9MtT8wrAa3sOOZbN93ZTFR5oM6TlDwuT2O6vu6oRA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2026, 3, 9, 22, 19, 23, 982, DateTimeKind.Local).AddTicks(826));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4768e246-9d29-4de6-aa4d-bf72a8715609", "AQAAAAIAAYagAAAAENlEPPADo1prSfJH9pLuWDhp0uAvC3YKFjFxrMj10ptZHsr7s0v/8YyYTRVDlvsZPQ==" });
        }
    }
}
