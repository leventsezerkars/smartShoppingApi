namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CustomerId).IsRequired();
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(e => e.Currency).HasConversion<string>().HasMaxLength(8).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt);

        builder.OwnsOne(e => e.TotalAmount, m =>
        {
            m.Property(x => x.Amount).HasPrecision(18, 4).IsRequired();
            m.Property(x => x.Currency).HasConversion<string>().HasMaxLength(8).IsRequired();
        });

        builder.Navigation(e => e.Items).HasField("_items").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
