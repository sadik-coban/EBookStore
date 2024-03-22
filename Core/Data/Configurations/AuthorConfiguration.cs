using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Configurations;
public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique(true);

        builder
            .HasQueryFilter(p => !p.IsSoftDeleted);

        builder
            .HasMany(p => p.Products)
            .WithMany(p => p.Authors);
    }
}