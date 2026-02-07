namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class BusinessLogConfiguration : IEntityTypeConfiguration<BusinessLog>
{
    public void Configure(EntityTypeBuilder<BusinessLog> builder)
    {
        builder.ToTable("BusinessLogs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.EntityName).HasMaxLength(128).IsRequired();
        builder.Property(e => e.EntityId);
        builder.Property(e => e.Action).HasMaxLength(128).IsRequired();
        builder.Property(e => e.ContextJson).HasMaxLength(4000);
        builder.Property(e => e.CorrelationId).HasMaxLength(64);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
    }
}
