using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Configurations;
public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique(true);

        builder
            .HasQueryFilter(p => !p.IsSoftDeleted);

        builder
            .HasMany(p => p.Products)
            .WithOne(p => p.Publisher)
            .HasForeignKey(p => p.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}