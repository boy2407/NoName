using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class CategoryTranslationConfiguration :IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.ToTable("CategoryTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.SeoDescription).HasMaxLength(500);
            builder.Property(x => x.SeoTitle).HasMaxLength(200);
            builder.Property(x => x.SeoAlias).HasMaxLength(200);
            builder.HasOne(x => x.Category).WithMany(c => c.CategoryTranslations).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.Language).WithMany(l => l.CategoryTranslations).HasForeignKey(x => x.LanguageId);
        }
    }
}
