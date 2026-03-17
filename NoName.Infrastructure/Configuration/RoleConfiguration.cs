using Microsoft.EntityFrameworkCore;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Infrastructure.Configuration
{
    public class RoleConfiguration :IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(x => x.Description).HasMaxLength(200).IsRequired(false);
        }
    }
}
