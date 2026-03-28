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
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.ToTable("ProductTags");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TagGroup).IsRequired().HasMaxLength(50);
            builder.HasMany(x => x.Products)
                   .WithMany(x => x.ProductTags)
                   .UsingEntity<ProductTagMapping>();
        }
    }
}
