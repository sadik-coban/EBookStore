using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations;

public class CarouselImageConfiguration : IEntityTypeConfiguration<CarouselImage>
{
    public void Configure(EntityTypeBuilder<CarouselImage> builder)
    {
        builder
            .Property(p => p.Image)
            .IsUnicode(false);
    }
}