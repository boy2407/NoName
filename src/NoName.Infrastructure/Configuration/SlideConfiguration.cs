using Microsoft.EntityFrameworkCore;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class SlideConfiguration : IEntityTypeConfiguration<Slide>
    {
         public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Slide> builder)
        {
            builder.ToTable("Slides");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Url).HasMaxLength(200);
            builder.Property(x => x.Image).HasMaxLength(200);
            builder.Property(x => x.SortOrder).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}
