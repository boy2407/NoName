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
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhysicalQuantity).HasDefaultValue(0);
            builder.Property(x => x.ReservedQuantity).HasDefaultValue(0);

            // Thiết lập quan hệ 1-1: 1 Variant chỉ có 1 bản ghi Inventory (1 kho)
            builder.HasOne(x => x.ProductVariant)
                   .WithOne(x => x.Inventory)
                   .HasForeignKey<Inventory>(x => x.ProductVariantId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Variant thì xóa luôn kho
        }
    }
}
