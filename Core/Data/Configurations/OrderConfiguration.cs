using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Core.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder
            .HasIndex(p => new { p.DateCreated })
            .IsDescending();

        builder
            .Property(p => p.DateCreated)
            .HasColumnType("smalldatetime");

        builder
            .HasMany(p => p.OrderDetails)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}