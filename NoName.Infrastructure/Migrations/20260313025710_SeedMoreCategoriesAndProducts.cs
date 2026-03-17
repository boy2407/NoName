using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NoName.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreCategoriesAndProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsShowOnHome", "ParentId", "SortOrder", "Status" },
                values: new object[,]
                {
                    { 3, true, 1, 1, 1 },
                    { 4, true, 1, 2, 1 },
                    { 5, true, 2, 1, 1 }
                });

            migrationBuilder.UpdateData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Áo nữ");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2026, 3, 13, 9, 57, 9, 182, DateTimeKind.Local).AddTicks(9643));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "DateModified", "IsActive", "OriginalPrice", "Price", "Stock", "ViewCount" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 100000m, 180000m, 50, 10 },
                    { 3, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 300000m, 450000m, 20, 5 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d60a807d-a3ef-4a9c-ba73-b6ffb21cae11"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ad752c1d-15f6-41ca-b177-2bbe4b3f3851", "AQAAAAIAAYagAAAAEC1OMD5FQE/gDfVI0LmtpxSV4FXRCw8jddAYKVle09QubXGfkdl+cs2bktLZAYEo1Q==" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsShowOnHome", "ParentId", "SortOrder", "Status" },
                values: new object[] { 6, false, 3, 1, 1 });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 5, 3, "vi-VN", "Áo sơ mi nam", "ao-so-mi-nam", "", "" },
                    { 6, 3, "en-US", "Men's Shirts", "mens-shirts", "", "" },
                    { 7, 4, "vi-VN", "Áo thun nam", "ao-thun-nam", "", "" },
                    { 8, 4, "en-US", "Men's T-Shirts", "mens-tshirts", "", "" }
                });

            migrationBuilder.InsertData(
                table: "ProductInCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 2 },
                    { 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "ProductTranslations",
                columns: new[] { "Id", "Description", "Details", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 4, "4-way stretch cotton", " Description for product", "en-US", "Men's Basic T-Shirt", 2, "mens-basic-tshirt", "Men's Basic T-Shirt", "" },
                    { 5, "Váy lụa nhẹ nhàng", "Mô tả sản phẩm", "vi-VN", "Váy hoa nhí", 3, "vay-hoa-nhi", "Váy hoa nhí", "" },
                    { 6, "Soft silk dress", "Mô tả sản phẩm", "en-US", "Floral Dress", 3, "floral-dress", "Floral Dress", "" }
                });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 9, 6, "vi-VN", "Sơ mi công sở", "so-mi-cong-so", "", "" },
                    { 10, 6, "en-US", "Office Shirts", "office-shirts", "", "" }
                });

            migrationBuilder.InsertData(
                table: "ProductInCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { 6, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Áo nứ");

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
        }
    }
}
