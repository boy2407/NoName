using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class ProductImageConfiguration:IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Caption).HasMaxLength(200);
            builder.Property(x => x.IsDefault).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.SortOrder).IsRequired();
            builder.Property(x => x.FileSize).IsRequired();
            builder.HasOne(x => x.Product).WithMany(p => p.ProductImages).HasForeignKey(x => x.ProductId);
        }
    }
}
