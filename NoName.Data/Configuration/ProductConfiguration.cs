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
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.OriginalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(x =>x.ViewCount).IsRequired().HasDefaultValue(0);

            
            builder.Navigation(x => x.ProductImages)
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .HasField("_productImages");
        }
    }
}
