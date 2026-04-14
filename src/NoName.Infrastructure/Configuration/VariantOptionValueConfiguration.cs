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
    public class VariantOptionValueConfiguration : IEntityTypeConfiguration<VariantOptionValue>
    {
        public void Configure(EntityTypeBuilder<VariantOptionValue> builder)
        {
            builder.ToTable("VariantOptionValues");
            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.Variant)
                       .WithMany(x => x.OptionValues)
                       .HasForeignKey(x => x.VariantId)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OptionValue)
                   .WithMany()
                   .HasForeignKey(x => x.OptionValueId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
