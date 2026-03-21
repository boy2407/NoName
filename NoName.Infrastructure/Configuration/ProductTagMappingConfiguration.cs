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
    public class ProductTagMappingConfiguration : IEntityTypeConfiguration<ProductTagMapping>
    {
        public void Configure(EntityTypeBuilder<ProductTagMapping> builder)
        {
            builder.ToTable("ProductTagMappings");

        
            builder.HasKey(x => new { x.ProductId, x.TagId });

            builder.HasOne(pt => pt.Product)
                   .WithMany(p => p.ProductTagMappings)
                   .HasForeignKey(pt => pt.ProductId);

            builder.HasOne(pt => pt.ProductTag)
                   .WithMany(t => t.ProductTagMappings)
                   .HasForeignKey(pt => pt.TagId);
        }
    }
}
