using NoName.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(

                new AppConfig() { Key = "HomeTitle", Value = "This is home page of NoName" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of NoName" },
                new AppConfig() { Key = "HomeDescription", Value = "This is description of NoName" }
            );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Enums.Status.Active,

                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Enums.Status.Active,

                 }
            );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                 new CategoryTranslation()
                 {
                     Id = 1,
                     CategoryId = 1,
                     Name = "Áo nam",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-nam",
                     SeoDescription = "Sản phẩm áo thời trang nam",
                     SeoTitle = "Sản phẩm áo thời trang nam"
                 },
                  new CategoryTranslation()
                  {
                      Id = 2,
                      CategoryId = 1,
                      Name = "Men shirt",
                      LanguageId = "en-US",
                      SeoAlias = "men-shirt",
                      SeoDescription = "shirt for men",
                      SeoTitle = "shirt for mem "
                  },
                   new CategoryTranslation()
                   {
                       Id = 3,
                       CategoryId = 2,
                       Name = "Áo nứ",
                       LanguageId = "vi-VN",
                       SeoAlias = "ao-nu",
                       SeoDescription = "Sản phẩm áo thời trang nữ",
                       SeoTitle = "Sản phẩm áo thời trang nữ"
                   }, new CategoryTranslation()
                   {
                       Id = 4,
                       CategoryId = 2,
                       Name = "Women shirt",
                       LanguageId = "en-US",
                       SeoAlias = "Women-shirt",
                       SeoDescription = "shirt for Women",
                       SeoTitle = "shirt for Women "
                   }
            );


            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000,
                    Price = 200000,
                    ViewCount = 0,
                    Stock = 0,

                }
                );
            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation()
                {   Id  = 1,
                    ProductId = 1,
                    Name = "Áo sơ mi nam trắng Việt Tiến",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                    SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                    SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                    Details = "Mô tả sản phẩm",
                    Description = "Áo sơ mi nam trắng Việt Tiến chất liệu cotton cao cấp, form slim-fit hiện đại, phù hợp công sở và sự kiện"
                },
                new ProductTranslation()
                {
                    Id = 2,
                    ProductId = 1,
                    Name = "Viet Tien shirt ",
                    LanguageId = "en-US",
                    SeoAlias = "viet-tien-shirt",
                    SeoDescription = "Viet Tien shirt",
                    SeoTitle = "Viet Tien shirt",
                    Details = " Description for product",
                    Description = "Premium white men's shirt from Viet Tien, made of high-quality cotton with a modern slim-fit design"
                }

            );

             modelBuilder.Entity<ProductInCategory>().HasData(
                 new ProductInCategory() { ProductId = 1, CategoryId = 1 }
             );


        }
    }
}
