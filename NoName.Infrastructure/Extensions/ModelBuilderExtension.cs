using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoName.Domain.Entities;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoName.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    private static string GenerateSlug(string phrase)
    {
        string str = phrase.ToLower().Trim();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Replace(" ", "-");
        return str;
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        // Use a fixed seed date so migrations remain stable
        var seedDate = new DateTime(2026, 1, 1);

        // --- 1. CONFIG CƠ BẢN ---
        modelBuilder.Entity<AppConfig>().HasData(
            new AppConfig { Key = "HomeTitle", Value = "NoName E-commerce" },
            new AppConfig { Key = "HomeKeyword", Value = "Fashion, AI, .NET 8" },
            new AppConfig { Key = "HomeDescription", Value = "Smart Fashion Store" }
        );

        modelBuilder.Entity<Language>().HasData(
            new Language { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
            new Language { Id = "en-US", Name = "English", IsDefault = false }
        );

        // --- 2. SEED TAGS & TRANSLATIONS (4x4 Matrix) ---
        var tags = new List<ProductTag>();
        var tagTranslations = new List<ProductTagTranslation>();
        int tagTransId = 1;

        var tagMatrix = new[]
        {
            new { Group = "Occasion", Vi = new[] { "Công sở", "Dạo phố", "Dự tiệc", "Thể thao" }, En = new[] { "Office", "Casual", "Party", "Sporty" } },
            new { Group = "Style", Vi = new[] { "Thanh lịch", "Năng động", "Tối giản", "Cá tính" }, En = new[] { "Elegant", "Dynamic", "Minimalist", "Edgy" } },
            new { Group = "Weather", Vi = new[] { "Mùa hè", "Mùa đông", "Thu đông", "Mọi thời tiết" }, En = new[] { "Summer", "Winter", "Autumn", "All-weather" } },
            new { Group = "Feature", Vi = new[] { "Thoáng khí", "Co giãn", "Chống nhăn", "Giữ nhiệt" }, En = new[] { "Breathable", "Stretchy", "Anti-wrinkle", "Thermal" } }
        };

        int tagIdCounter = 1;
        foreach (var group in tagMatrix)
        {
            for (int i = 0; i < 4; i++)
            {
                int tid = tagIdCounter++;
                tags.Add(new ProductTag { Id = tid, TagGroup = group.Group, Name = group.En[i] });
                tagTranslations.Add(new ProductTagTranslation { Id = tagTransId++, TagId = tid, LanguageId = "vi-VN", Name = group.Vi[i] });
                tagTranslations.Add(new ProductTagTranslation { Id = tagTransId++, TagId = tid, LanguageId = "en-US", Name = group.En[i] });
            }
        }
        modelBuilder.Entity<ProductTag>().HasData(tags);
        modelBuilder.Entity<ProductTagTranslation>().HasData(tagTranslations);

        // --- 3. SEED CATEGORIES ---
        var faker = new Faker("vi");
        Randomizer.Seed = new Random(8675309);

        var materials = new[] { "Cotton hữu cơ", "Sợi tre Bamboo", "Lụa tơ tằm", "Denim nguyên bản", "Len Merino", "Vải Linen" };
        var adjectives = new[] { "Premium", "Luxury", "Essential", "Daily", "Signature" };
        var colors = new[] { "Đen", "Trắng", "Xanh Navy", "Xám ghi", "Be", "Hồng" };

        var colorMap = new Dictionary<string, string>
        {
            { "Đen", "Black" }, { "Trắng", "White" }, { "Xanh Navy", "Navy Blue" },
            { "Xám ghi", "Gray" }, { "Be", "Beige" }, { "Hồng", "Pink" }
        };

        var menuStructure = new[]
        {
            new { Id = 1, Name = "Nam", Subs = new[] { "Áo thun", "Áo sơ mi", "Áo len", "Áo khoác", "Áo polo", "Quần jean", "Quần short", "Quần dài" } },
            new { Id = 2, Name = "Nữ", Subs = new[] { "Áo thun", "Áo sơ mi", "Áo len", "Áo khoác", "Áo polo", "Áo dài", "Quần jean", "Quần short", "Quần dài" } }
        };

        var tagMappingLogic = new Dictionary<string, int[]> {
            { "Áo thun", new[] { 2, 6, 9, 13, 14 } }, { "Áo sơ mi", new[] { 1, 5, 12, 15 } },
            { "Áo polo", new[] { 1, 6, 12, 15 } },   { "Quần short", new[] { 2, 6, 9, 14 } },
            { "Áo khoác", new[] { 2, 8, 11, 15 } },  { "Áo len", new[] { 2, 5, 10, 16 } },
            { "Quần jean", new[] { 2, 8, 12, 14 } }, { "Quần dài", new[] { 1, 5, 12, 15 } },
            { "Áo dài", new[] { 3, 5, 12, 13 } }
        };

        var categories = new List<Category>();
        var categoryTrans = new List<CategoryTranslation>();
        var products = new List<Product>();
        var productTrans = new List<ProductTranslation>();
        var productTagMappings = new List<ProductTagMapping>();

        var productOptions = new List<ProductOption>();
        var productOptionTranslations = new List<ProductOptionTranslation>();
        var productOptionValues = new List<ProductOptionValue>();
        var productOptionValueTranslations = new List<ProductOptionValueTranslation>();

        var variants = new List<ProductVariant>();
        var variantOptionValues = new List<VariantOptionValue>();
        var inventories = new List<Inventory>();
        var productInCategories = new List<ProductInCategory>();

        int catIdCounter = 10;
        int catTransIdCounter = 1;
        int pIdCounter = 1;
        int tIdCounter = 1;
        int vIdCounter = 1;
        int vovIdCounter = 1;

        int optIdCounter = 1;
        int optTransIdCounter = 1;
        int optValIdCounter = 1;
        int optValTransIdCounter = 1;

        foreach (var parent in menuStructure)
        {
            // 1. Parent Category
            categories.Add(new Category { Id = parent.Id, Status = Status.Active, IsShowOnHome = true });
            categoryTrans.Add(new CategoryTranslation
            {
                Id = catTransIdCounter++,
                CategoryId = parent.Id,
                LanguageId = "vi-VN",
                Name = $"Thời trang {parent.Name}",
                SeoAlias = GenerateSlug(parent.Name),
                SeoDescription = $"Bộ sưu tập thời trang {parent.Name} cao cấp 2026.",
                SeoTitle = $"Thời Trang {parent.Name} | NoName"
            });

            foreach (var sub in parent.Subs)
            {
                // 2. Sub Category
                int currentSubId = catIdCounter++;
                categories.Add(new Category { Id = currentSubId, ParentId = parent.Id, Status = Status.Active });
                categoryTrans.Add(new CategoryTranslation
                {
                    Id = catTransIdCounter++,
                    CategoryId = currentSubId,
                    LanguageId = "vi-VN",
                    Name = $"{sub} {parent.Name}",
                    SeoAlias = GenerateSlug($"{sub}-{parent.Name}"),
                    SeoDescription = $"Bộ sưu tập {sub} cho {parent.Name} - NoName.",
                    SeoTitle = $"{sub} {parent.Name} | NoName"
                });

                // 3. Products
                for (int i = 1; i <= 5; i++)
                {
                    int currentPId = pIdCounter++;
                    var mat = faker.PickRandom(materials);
                    var name = $"{sub} {parent.Name} {faker.PickRandom(adjectives)} {i}";

                    products.Add(new Product { Id = currentPId, DateCreated = seedDate, IsActive = true });
                    productInCategories.Add(new ProductInCategory { ProductId = currentPId, CategoryId = currentSubId });

                    if (tagMappingLogic.TryGetValue(sub, out var assignedTags))
                    {
                        foreach (var tagId in assignedTags)
                        {
                            productTagMappings.Add(new ProductTagMapping { ProductId = currentPId, TagId = tagId });
                        }
                    }

                    productTrans.Add(new ProductTranslation
                    {
                        Id = tIdCounter++,
                        ProductId = currentPId,
                        LanguageId = "vi-VN",
                        Name = name,
                        SeoAlias = $"{GenerateSlug(name)}-{currentPId}",
                        Description = $"{name} - Chất liệu {mat}. Phù hợp cho: {sub}.",
                        Details = $"Chất liệu: {mat}. Sản xuất tại Việt Nam.",
                        SeoDescription = $"{name} - Chất liệu {mat}. Mua ngay tại NoName.",
                        SeoTitle = $"{name} | NoName"
                    });

                    // --- OPTIONS MULTILINGUAL SEEDING ---
                    int sOptId = optIdCounter++;
                    int cOptId = optIdCounter++;

                    productOptions.Add(new ProductOption { Id = sOptId, ProductId = currentPId });
                    productOptions.Add(new ProductOption { Id = cOptId, ProductId = currentPId });

                    productOptionTranslations.Add(new ProductOptionTranslation { Id = optTransIdCounter++, OptionId = sOptId, LanguageId = "vi-VN", Name = "Kích cỡ" });
                    productOptionTranslations.Add(new ProductOptionTranslation { Id = optTransIdCounter++, OptionId = sOptId, LanguageId = "en-US", Name = "Size" });

                    productOptionTranslations.Add(new ProductOptionTranslation { Id = optTransIdCounter++, OptionId = cOptId, LanguageId = "vi-VN", Name = "Màu sắc" });
                    productOptionTranslations.Add(new ProductOptionTranslation { Id = optTransIdCounter++, OptionId = cOptId, LanguageId = "en-US", Name = "Color" });

                    // --- OPTION VALUES MULTILINGUAL SEEDING ---
                    var sizes = new[] { "M", "L", "XL" };
                    var sizeValueIds = new List<int>();

                    foreach (var s in sizes)
                    {
                        int svId = optValIdCounter++;
                        productOptionValues.Add(new ProductOptionValue { Id = svId, OptionId = sOptId });

                        productOptionValueTranslations.Add(new ProductOptionValueTranslation { Id = optValTransIdCounter++, ProductOptionValueId = svId, LanguageId = "vi-VN", Name = s });
                        productOptionValueTranslations.Add(new ProductOptionValueTranslation { Id = optValTransIdCounter++, ProductOptionValueId = svId, LanguageId = "en-US", Name = s });

                        sizeValueIds.Add(svId);
                    }

                    var chosenColors = faker.PickRandom(colors, faker.Random.Int(2, 3)).ToList();
                    var colorValueIds = new List<int>();

                    foreach (var col in chosenColors)
                    {
                        int cvId = optValIdCounter++;
                        productOptionValues.Add(new ProductOptionValue { Id = cvId, OptionId = cOptId });

                        colorMap.TryGetValue(col, out var enColor);
                        productOptionValueTranslations.Add(new ProductOptionValueTranslation { Id = optValTransIdCounter++, ProductOptionValueId = cvId, LanguageId = "vi-VN", Name = col });
                        productOptionValueTranslations.Add(new ProductOptionValueTranslation { Id = optValTransIdCounter++, ProductOptionValueId = cvId, LanguageId = "en-US", Name = enColor ?? col });

                        colorValueIds.Add(cvId);
                    }

                    // --- VARIANTS ---
                    foreach (var sizeId in sizeValueIds)
                    {
                        foreach (var colorValId in colorValueIds)
                        {
                            int currentVId = vIdCounter++;
                            decimal originalPrice = faker.Random.Int(20, 80) * 10000m;
                            decimal price = Math.Round(originalPrice * faker.Random.Decimal(0.75m, 0.98m), 2);

                            variants.Add(new ProductVariant { Id = currentVId, ProductId = currentPId, SKU = $"SKU-{currentPId}-{currentVId}", Price = price, OriginalPrice = originalPrice, CreatedAt = seedDate });
                            inventories.Add(new Inventory { Id = currentVId, ProductVariantId = currentVId, PhysicalQuantity = faker.Random.Int(20, 200), ReservedQuantity = faker.Random.Int(0, 20), LastUpdated = seedDate });

                            variantOptionValues.Add(new VariantOptionValue { Id = vovIdCounter++, VariantId = currentVId, OptionValueId = sizeId });
                            variantOptionValues.Add(new VariantOptionValue { Id = vovIdCounter++, VariantId = currentVId, OptionValueId = colorValId });
                        }
                    }
                }
            }
        }

        // --- 4. APPLY PRODUCT DATA ---
        modelBuilder.Entity<ProductTagMapping>().HasData(productTagMappings);
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<CategoryTranslation>().HasData(categoryTrans);
        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<ProductTranslation>().HasData(productTrans);

        // Apply Options & Translations
        modelBuilder.Entity<ProductOption>().HasData(productOptions);
        modelBuilder.Entity<ProductOptionTranslation>().HasData(productOptionTranslations);
        modelBuilder.Entity<ProductOptionValue>().HasData(productOptionValues);
        modelBuilder.Entity<ProductOptionValueTranslation>().HasData(productOptionValueTranslations);

        modelBuilder.Entity<ProductVariant>().HasData(variants);
        modelBuilder.Entity<VariantOptionValue>().HasData(variantOptionValues);
        modelBuilder.Entity<Inventory>().HasData(inventories);
        modelBuilder.Entity<ProductInCategory>().HasData(productInCategories);

        // --- 5. SEED IDENTITY (USER/ROLE) ---
        var adminId = Guid.Parse("D60A807D-A3EF-4A9C-BA73-B6FFB21CAE11");
        var adminRoleId = Guid.Parse("A1B2C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D");

        modelBuilder.Entity<Role>().HasData(new Role { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "ROLE-ADMIN-0001" });

        var hasher = new PasswordHasher<User>();
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@noname.com",
            NormalizedEmail = "ADMIN@NONAME.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "admin@"),
            SecurityStamp = "D60A807D-A3EF-4A9C-BA73-B6FFB21CAE11",
            ConcurrencyStamp = "A1B2C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D",
            FirstName = "Leon",
            LastName = "Developer"
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = adminRoleId, UserId = adminId });

        // --- 6. SEED SLIDES ---
        modelBuilder.Entity<Slide>().HasData(
            Enumerable.Range(1, 3).Select(i => new Slide { Id = i, Name = $"Banner {i}", Description = $"Banner {i} mô tả", Url = "#", SortOrder = i, Image = $"/img/banner-{i}.png", Status = Status.Active })
        );

        // --- 7. SEED PROMOTIONS ---
        var promotions = new List<Promotion>();
        var promotionProducts = new List<PromotionProduct>();
        int promoIdCounter = 1;

        for (int i = 1; i <= 3; i++)
        {
            promotions.Add(new Promotion
            {
                Id = promoIdCounter,
                Name = $"Giảm giá đặc biệt cho sản phẩm {i}",
                FromDate = seedDate.AddDays(faker.Random.Int(-30, -1)),
                ToDate = seedDate.AddDays(faker.Random.Int(1, 30)),
                DiscountPercent = faker.Random.Int(5, 20),
                DiscountAmount = faker.Random.Bool() ? (decimal?)faker.Random.Int(1000, 10000) : null,
                ApplyForAll = faker.Random.Bool(),
                IsActive = true
            });

            var assignedProductIds = products.Select(p => p.Id).ToList();
            var selectedProductIds = faker.PickRandom(assignedProductIds, faker.Random.Int(1, 3));
            foreach (var productId in selectedProductIds)
            {
                promotionProducts.Add(new PromotionProduct { PromotionId = promoIdCounter, ProductId = productId });
            }

            promoIdCounter++;
        }

        modelBuilder.Entity<Promotion>().HasData(promotions);
        modelBuilder.Entity<PromotionProduct>().HasData(promotionProducts);
    }
}