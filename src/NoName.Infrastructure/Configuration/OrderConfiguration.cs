using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.ShipName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShipEmail).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(x => x.TotalAmount).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
        }
    }
}
