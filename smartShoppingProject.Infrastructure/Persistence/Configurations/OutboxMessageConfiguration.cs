namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Type).HasMaxLength(512).IsRequired();
        builder.Property(e => e.Payload).IsRequired();
        builder.Property(e => e.OccurredOn).IsRequired();
        builder.Property(e => e.ProcessedOn);
    }
}
