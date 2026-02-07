namespace smartShoppingProject.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Domain.Common;
using smartShoppingProject.Domain.Events;
using smartShoppingProject.Domain.Entities;
using System.Text.Json;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<ApplicationLog> ApplicationLogs => Set<ApplicationLog>();
    public DbSet<BusinessLog> BusinessLogs => Set<BusinessLog>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await WriteDomainEventsToOutboxAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task WriteDomainEventsToOutboxAsync(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entries)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var outbox = OutboxMessage.Create(
                    domainEvent.GetType().AssemblyQualifiedName ?? domainEvent.GetType().FullName ?? nameof(IDomainEvent),
                    JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    domainEvent.OccurredOn);
                await OutboxMessages.AddAsync(outbox, cancellationToken);
            }
            entity.ClearDomainEvents();
        }
    }
}
