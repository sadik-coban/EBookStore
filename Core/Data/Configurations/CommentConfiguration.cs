using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasIndex(p => new { p.DateCreated })
            .IsDescending();

        builder
            .Property(p => p.DateCreated)
            .HasColumnType("smalldatetime");

        builder
            .HasMany(p => p.CommentImages)
            .WithOne(p => p.Comment)
            .OnDelete(DeleteBehavior.Cascade);
    }
}