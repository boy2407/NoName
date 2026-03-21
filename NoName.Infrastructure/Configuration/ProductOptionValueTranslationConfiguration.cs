using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;

namespace NoName.Infrastructure.Configurations;

public class ProductOptionValueTranslationConfiguration : IEntityTypeConfiguration<ProductOptionValueTranslation>
{
    public void Configure(EntityTypeBuilder<ProductOptionValueTranslation> builder)
    {
        builder.ToTable("ProductOptionValueTranslations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.LanguageId).IsRequired().HasMaxLength(5);

        // Đảm bảo 1 Value chỉ có 1 bản dịch cho 1 ngôn ngữ cụ thể
        builder.HasIndex(x => new { x.ProductOptionValueId, x.LanguageId }).IsUnique();

        builder.HasOne(x => x.ProductOptionValue)
               .WithMany(x => x.ProductOptionValueTranslations)
               .HasForeignKey(x => x.ProductOptionValueId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}