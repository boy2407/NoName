using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;

namespace NoName.Infrastructure.Configurations;

public class ProductOptionTranslationConfiguration : IEntityTypeConfiguration<ProductOptionTranslation>
{
    public void Configure(EntityTypeBuilder<ProductOptionTranslation> builder)
    {
        builder.ToTable("ProductOptionTranslations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.LanguageId).IsRequired().HasMaxLength(5);

        // Đảm bảo 1 Option chỉ có 1 bản dịch cho 1 ngôn ngữ cụ thể
        builder.HasIndex(x => new { x.OptionId, x.LanguageId }).IsUnique();

        builder.HasOne(x => x.Option)
               .WithMany(x => x.ProductOptionTranslations)
               .HasForeignKey(x => x.OptionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}