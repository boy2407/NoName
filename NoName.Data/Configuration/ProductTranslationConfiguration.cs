using Microsoft.EntityFrameworkCore;
using NoName.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Data.Configuration
{
    public class ProductTranslationConfiguration: IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.ToTable("ProductTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Details).HasMaxLength(500);
            builder.Property(x => x.SeoDescription).HasMaxLength(500);
            builder.Property(x => x.SeoTitle).HasMaxLength(200);
            builder.Property(x => x.SeoAlias).HasMaxLength(200);
            builder.HasOne(x => x.Product).WithMany(p => p.ProductTranslations).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.Language).WithMany(l => l.ProductTranslations).HasForeignKey(x => x.LanguageId);
        }
    }
}
