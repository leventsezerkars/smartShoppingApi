namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(2000).IsRequired();
        builder.Property(e => e.StockQuantity).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt);

        builder.OwnsOne(e => e.Price, m =>
        {
            m.Property(x => x.Amount).HasPrecision(18, 4).IsRequired();
            m.Property(x => x.Currency).HasConversion<string>().HasMaxLength(8).IsRequired();
        });
    }
}
