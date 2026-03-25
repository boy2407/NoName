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
    public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.ToTable("ProductOptionValues");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Option)
               .WithMany(x => x.Values)
               .HasForeignKey(x => x.OptionId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
