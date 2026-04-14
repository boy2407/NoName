using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderWithTotalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "TotalAmount",
                value: 3341000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "TotalAmount",
                value: 3960000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3,
                column: "TotalAmount",
                value: 7613000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4,
                column: "TotalAmount",
                value: 9034000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5,
                column: "TotalAmount",
                value: 7338000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 6,
                column: "TotalAmount",
                value: 1250000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 7,
                column: "TotalAmount",
                value: 4397000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 8,
                column: "TotalAmount",
                value: 3542000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 9,
                column: "TotalAmount",
                value: 5423000m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 10,
                column: "TotalAmount",
                value: 3938000m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMdaPSdJMBZU2/FPpP9DefJ4oUyrzLWTDRWUqK9MA0ZlswgdB2+S44zp83m2HhVyEw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8091"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGMutr4+Ty5X9qyvTSVLEr8wNGMs/qyOBX0BbygFerAyPLediiLYg/1Y5ycHc0PXaQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8091a2"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH/9tnVggZpqbu+KMSdI1kR5SryeB2dB9jBHuy//kKSdcg940nZtpEuBwPECBAQVTg==");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELCx7u2fEw1wvklPtVyp9lxMV00ls6pUlqMMDQV8h+bYMoK0KCfsqQHctOMD+y6rAw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8091"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEdovPtn3jL4ck5nwX7XBw9/zHTG3M2/ccMA7IjzTh4WcpSZzZ8Uc7vDyisKj8iw7g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8091a2"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM7f+zmxhfhFTKzNbYonGHGal68HCGIWG4OfK0iBfkKzGQDKgS0s95O84dTK+r8tfQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
