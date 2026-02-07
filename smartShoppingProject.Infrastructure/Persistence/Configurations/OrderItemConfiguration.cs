namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(e => e.Id);

        builder.Property<Guid>("OrderId").IsRequired();
        builder.HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Quantity).IsRequired();

        builder.OwnsOne(e => e.UnitPrice, m =>
        {
            m.Property(x => x.Amount).HasPrecision(18, 4).IsRequired();
            m.Property(x => x.Currency).HasConversion<string>().HasMaxLength(8).IsRequired();
        });
    }
}
