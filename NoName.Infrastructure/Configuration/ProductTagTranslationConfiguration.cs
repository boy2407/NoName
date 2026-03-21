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
    public class ProductTagTranslationConfiguration : IEntityTypeConfiguration<ProductTagTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTagTranslation> builder)
        {
            builder.ToTable("ProductTagTranslations");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            // Match Language.Id default length (nvarchar(450)) to avoid FK type mismatch
            builder.Property(x => x.LanguageId).IsRequired().HasMaxLength(450);

            builder.HasOne(x => x.ProductTag)
                   .WithMany(x => x.TagTranslations)
                   .HasForeignKey(x => x.TagId);

            builder.HasOne(x => x.Language)
                   .WithMany(l => l.ProductTagTranslations)
                   .HasForeignKey(x => x.LanguageId);
        }
    }
}
