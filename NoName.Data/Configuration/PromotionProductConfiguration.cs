using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class PromotionProductConfiguration : IEntityTypeConfiguration<PromotionProduct>
    {
        public void Configure(EntityTypeBuilder<PromotionProduct> builder)
        {
            builder.ToTable("PromotionProducts");
            builder.HasKey(x => new { x.PromotionId, x.ProductId });
            builder.HasOne(x => x.Promotion).WithMany(p => p.PromotionProducts).HasForeignKey(x => x.PromotionId);
            builder.HasOne(x => x.Product).WithMany(p => p.PromotionProducts).HasForeignKey(x => x.ProductId);
        }
    }
}
