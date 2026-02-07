namespace smartShoppingProject.Infrastructure.Messaging;

using MassTransit;
using Microsoft.Extensions.Logging;
using smartShoppingProject.Application.Abstractions.Messaging;
using smartShoppingProject.Domain.Events;
using System.Text.Json;

/// <summary>
/// Domain event'leri MassTransit ile RabbitMQ'ya yayımlar.
/// </summary>
internal sealed class RabbitMqEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RabbitMqEventBus> _logger;

    public RabbitMqEventBus(IPublishEndpoint publishEndpoint, ILogger<RabbitMqEventBus> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (domainEvents.Count == 0) return;

        foreach (var evt in domainEvents)
        {
            var envelope = new DomainEventEnvelope(
                evt.GetType().AssemblyQualifiedName ?? evt.GetType().FullName ?? nameof(IDomainEvent),
                JsonSerializer.Serialize(evt, evt.GetType()),
                evt.OccurredOn);

            await _publishEndpoint.Publish(envelope, cancellationToken);
            _logger.LogInformation("Event RabbitMQ'ya gönderildi: {EventType}", evt.GetType().Name);
        }
    }
}
