using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTransactionWithPayURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayUrl",
                table: "Transactions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHABFBCZxknl+JtSyv/eFnWS9Fyoh1VbTDvEtDDsphL90byajkinTNyt6ak0m+yadg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8091"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPwtDnFV/Ks2bcRvhygBMMgBWqhX5coMaSk05luuWpVPKwq8py2fu+95P79jJYOpXA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8091a2"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPpOZC0hCJNU8xrvM14xKltGYKYG+vdKXOWC0fRpA6L3tmzY+TrxV1QOLEIThXJvVw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayUrl",
                table: "Transactions");

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
        }
    }
}
