using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class AppConfigConfiguration : IEntityTypeConfiguration<AppConfig>
    {
      public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            builder.ToTable("AppConfig");
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Value).IsRequired();
            
        }
    }
}
