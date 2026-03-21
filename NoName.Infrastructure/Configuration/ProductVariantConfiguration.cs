using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Configuration
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SKU).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.SKU).IsUnique();

            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.OriginalPrice).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.OptionValues)
               .WithOne(x => x.Variant)
               .HasForeignKey(x => x.VariantId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
