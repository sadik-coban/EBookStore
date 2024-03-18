using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations;

public class CommentImageConfiguration : IEntityTypeConfiguration<CommentImage>
{
    public void Configure(EntityTypeBuilder<CommentImage> builder)
    {
        builder
            .Property(p => p.Image)
            .IsUnicode(false);

        builder
            .HasOne(p => p.Comment)
            .WithMany(p => p.CommentImages)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}