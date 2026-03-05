using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using NoName.Data.Entities;

namespace NoName.Data.Configuration
{
    public class TransactionConfiguartion : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TransactionDate).IsRequired();
            builder.Property(x => x.ExternalTransactionId).HasMaxLength(200);
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Fee).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Result).HasMaxLength(200);
            builder.Property(x => x.Message).HasMaxLength(500);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Provider).HasMaxLength(100);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.Transactions).HasForeignKey(x => x.UserId);

         }
    }
}
