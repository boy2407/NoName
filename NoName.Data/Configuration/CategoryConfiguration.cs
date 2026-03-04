using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Data.Entities;
using NoName.Data.Enums;
namespace NoName.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SortOrder).IsRequired();
            builder.Property(x => x.IsShowOnHome).IsRequired();
            builder.Property(x => x.ParentId).IsRequired(false);
            builder.Property(x => x.Status).HasDefaultValue(Status.Active);
        }
    }
}
