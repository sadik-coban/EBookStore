using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations;

public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique(true);

        builder
            .HasQueryFilter(p => !p.IsSoftDeleted);

        builder
            .HasMany(p => p.Products)
            .WithMany(p => p.Catalogs);
    }
}