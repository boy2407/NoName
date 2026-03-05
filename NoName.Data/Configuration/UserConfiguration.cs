using Microsoft.EntityFrameworkCore;
using NoName.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Data.Configuration
{
    public class UserConfiguration :IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(x => x.FirstName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Dob).IsRequired();
        }
    }
}
