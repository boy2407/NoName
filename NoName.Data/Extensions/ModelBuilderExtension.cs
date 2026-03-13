using Microsoft.AspNetCore.Identity;
using NoName.Domain.Entities;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Extensions
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
                // Level 1: Id 1 (Áo nam), Id 2 (Áo nữ) - Đã có ở trên
                new Category() { Id = 1, ParentId = null, SortOrder = 1, IsShowOnHome = true, Status = Status.Active }, // Áo sơ mi nam
                new Category() { Id = 2, ParentId = null, SortOrder = 2, IsShowOnHome = true, Status = Status.Active }, // Áo thun nam

                // Level 2: Con của Áo nam (Id=1)
                new Category() { Id = 3, ParentId = 1, SortOrder = 1, IsShowOnHome = true, Status = Status.Active }, // Áo sơ mi nam
                new Category() { Id = 4, ParentId = 1, SortOrder = 2, IsShowOnHome = true, Status = Status.Active }, // Áo thun nam

                // Level 2: Con của Áo nữ (Id=2)
                new Category() { Id = 5, ParentId = 2, SortOrder = 1, IsShowOnHome = true, Status = Status.Active }, // Váy nữ

                // Level 3: Con của Áo sơ mi nam (Id=3)
                new Category() { Id = 6, ParentId = 3, SortOrder = 1, IsShowOnHome = false, Status = Status.Active } // Sơ mi công sở
            );

            modelBuilder.Entity<CategoryTranslation>().HasData(

                // Translate Id 1
                new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Áo nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men shirt", LanguageId = "en-US", SeoAlias = "men-shirt", SeoDescription = "shirt for men", SeoTitle = "shirt for mem " },

                // Translate Id 2
                new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang nữ" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women shirt", LanguageId = "en-US", SeoAlias = "Women-shirt", SeoDescription = "shirt for Women", SeoTitle = "shirt for Women " },

                // Translate Id 3
                new CategoryTranslation() { Id = 5, CategoryId = 3, Name = "Áo sơ mi nam", LanguageId = "vi-VN", SeoAlias = "ao-so-mi-nam", SeoDescription ="", SeoTitle = "", },
                new CategoryTranslation() { Id = 6, CategoryId = 3, Name = "Men's Shirts", LanguageId = "en-US", SeoAlias = "mens-shirts", SeoDescription ="", SeoTitle = "", },

                // Translate Id 4
                new CategoryTranslation() { Id = 7, CategoryId = 4, Name = "Áo thun nam", LanguageId = "vi-VN", SeoAlias = "ao-thun-nam", SeoDescription ="", SeoTitle = "", },
                new CategoryTranslation() { Id = 8, CategoryId = 4, Name = "Men's T-Shirts", LanguageId = "en-US", SeoAlias = "mens-tshirts", SeoDescription ="", SeoTitle = "", },

                // Translate Id 6
                new CategoryTranslation() { Id = 9, CategoryId = 6, Name = "Sơ mi công sở", LanguageId = "vi-VN", SeoAlias = "so-mi-cong-so", SeoDescription ="", SeoTitle = "", },
                new CategoryTranslation() { Id = 10, CategoryId = 6, Name = "Office Shirts", LanguageId = "en-US", SeoAlias = "office-shirts", SeoDescription ="", SeoTitle = "", }
            );


            modelBuilder.Entity<Product>().HasData(

                new Product() { Id = 1, DateCreated = DateTime.Now, OriginalPrice = 100000, Price = 200000, ViewCount = 0, Stock = 0, },

                new Product() { Id = 2, DateCreated = new DateTime(2026, 03, 11), OriginalPrice = 100000, Price = 180000, Stock = 50, ViewCount = 10 },

                new Product() { Id = 3, DateCreated = new DateTime(2026, 03, 12), OriginalPrice = 300000, Price = 450000, Stock = 20, ViewCount = 5 }

             );

            modelBuilder.Entity<ProductTranslation>().HasData(

                new ProductTranslation()
                {
                    Id = 1,
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
                },
                new ProductTranslation()
                {
                    Id = 4,
                    ProductId = 2,
                    LanguageId = "en-US",
                    Name = "Men's Basic T-Shirt",
                    SeoAlias = "mens-basic-tshirt",
                    SeoDescription = "Men's Basic T-Shirt",
                    SeoTitle="",
                    Details = " Description for product",
                    Description = "4-way stretch cotton"
                },
                new ProductTranslation()
                {
                    Id = 5,
                    ProductId = 3,
                    LanguageId = "vi-VN",
                    Name = "Váy hoa nhí",
                    SeoAlias = "vay-hoa-nhi",
                    SeoDescription = "Váy hoa nhí",
                    SeoTitle = "",
                    Details = "Mô tả sản phẩm",
                    Description = "Váy lụa nhẹ nhàng"
                },
                new ProductTranslation()
                {
                    Id = 6,
                    ProductId = 3,
                    LanguageId = "en-US",
                    Name = "Floral Dress",
                    SeoAlias = "floral-dress",
                    SeoDescription ="Floral Dress",
                    SeoTitle = "",
                    Details = "Mô tả sản phẩm",
                    Description = "Soft silk dress"
                }
            );


            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }, // sơ mi VT -> Áo nam
                new ProductInCategory() { ProductId = 1, CategoryId = 3 }, // sơ mi VT -> Áo sơ mi nam
                new ProductInCategory() { ProductId = 1, CategoryId = 6 }, // sơ mi VT -> Sơ mi công sở 
                new ProductInCategory() { ProductId = 2, CategoryId = 1 }, // Áo thun -> Áo nam
                new ProductInCategory() { ProductId = 2, CategoryId = 4 }, // Áo thun -> Áo thun nam
                new ProductInCategory() { ProductId = 3, CategoryId = 2 }, // Váy -> Áo nữ
                new ProductInCategory() { ProductId = 3, CategoryId = 5 }  // Váy -> Váy nữ

            );

            var roleId = new Guid("4CCF9361-16BC-4224-99C6-B87223226EA5");
            var adminId = new Guid("D60A807D-A3EF-4A9C-BA73-B6FFB21CAE11");
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "nguyentrongnghia7949@gmail.com",
                NormalizedEmail = "nguyentrongnghia7949@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "trnghia1234@"),
                SecurityStamp = string.Empty,
                FirstName = "Nghia",
                LastName = "Trong",
                Dob = new DateTime(2026, 03, 10)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { Id = 1, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 1, Url = "#", Image = "/themes/images/carousel/1.png", Status = Status.Active },
              new Slide() { Id = 2, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 2, Url = "#", Image = "/themes/images/carousel/2.png", Status = Status.Active },
              new Slide() { Id = 3, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 3, Url = "#", Image = "/themes/images/carousel/3.png", Status = Status.Active },
              new Slide() { Id = 4, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 4, Url = "#", Image = "/themes/images/carousel/4.png", Status = Status.Active },
              new Slide() { Id = 5, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 5, Url = "#", Image = "/themes/images/carousel/5.png", Status = Status.Active },
              new Slide() { Id = 6, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 6, Url = "#", Image = "/themes/images/carousel/6.png", Status = Status.Active }
              );

        }
    }
}
