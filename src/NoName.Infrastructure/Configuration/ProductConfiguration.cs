using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ViewCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            // Variants mapping
            builder.HasMany(x => x.ProductVariants)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

            builder.Navigation(x => x.ProductVariants)
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .HasField("_productVariants");

            // Mapping bảng trung gian Tag
            builder.Navigation(x => x.ProductTagMappings)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            // Options 1-N
            builder.HasMany(x => x.Options)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
