namespace smartShoppingProject.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using smartShoppingProject.Domain.Entities;

internal sealed class ApplicationLogConfiguration : IEntityTypeConfiguration<ApplicationLog>
{
    public void Configure(EntityTypeBuilder<ApplicationLog> builder)
    {
        builder.ToTable("ApplicationLogs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Level).HasMaxLength(32).IsRequired();
        builder.Property(e => e.Message).HasMaxLength(4000).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.ExceptionMessage).HasMaxLength(2000);
    }
}
