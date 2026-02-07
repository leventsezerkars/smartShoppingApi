namespace smartShoppingProject.Infrastructure.BackgroundServices;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using smartShoppingProject.Application.Abstractions.Messaging;
using smartShoppingProject.Domain.Events;
using smartShoppingProject.Infrastructure.Persistence;
using smartShoppingProject.Domain.Entities;

/// <summary>
/// ProcessedOn = null olan OutboxMessage kayıtlarını alır, IEventBus ile yayımlar, başarıda ProcessedOn doldurur.
/// Retry ve hata yönetimi: başarısız kayıtlar sonraki döngüde tekrar işlenir.
/// </summary>
internal sealed class OutboxMessageProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxMessageProcessor> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);
    private static readonly int BatchSize = 20;

    public OutboxMessageProcessor(IServiceProvider serviceProvider, ILogger<OutboxMessageProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox işleme döngüsünde hata");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        var pending = await context.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.OccurredOn)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var message in pending)
        {
            try
            {
                var domainEvent = DeserializeEvent(message.Type, message.Payload);
                if (domainEvent is not null)
                {
                    await eventBus.PublishAsync(new[] { domainEvent }, cancellationToken);
                }

                message.MarkProcessed();
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Outbox mesajı işlenemedi: {MessageId}, Type: {Type}", message.Id, message.Type);
            }
        }
    }

    private static IDomainEvent? DeserializeEvent(string typeName, string payload)
    {
        var type = Type.GetType(typeName);
        if (type is null) return null;

        return JsonSerializer.Deserialize(payload, type) as IDomainEvent;
    }
}
