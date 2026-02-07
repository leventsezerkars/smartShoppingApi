namespace smartShoppingProject.Infrastructure.Messaging;

using System.Text.Json;
using MediatR;
using MassTransit;
using Microsoft.Extensions.Logging;
using smartShoppingProject.Application.Abstractions.Events;
using smartShoppingProject.Domain.Events;

/// <summary>
/// RabbitMQ'dan gelen DomainEventEnvelope mesajlarını alır, deserialize edip Application'daki MediatR handler'lara gönderir.
/// Böylece InMemory ve RabbitMQ aynı handler'ları kullanır.
/// </summary>
public sealed class DomainEventEnvelopeConsumer : IConsumer<DomainEventEnvelope>
{
    private readonly IMediator _mediator;
    private readonly IDomainEventToNotificationMapper _mapper;
    private readonly ILogger<DomainEventEnvelopeConsumer> _logger;

    public DomainEventEnvelopeConsumer(
        IMediator mediator,
        IDomainEventToNotificationMapper mapper,
        ILogger<DomainEventEnvelopeConsumer> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DomainEventEnvelope> context)
    {
        var envelope = context.Message;
        var domainEvent = Deserialize(envelope.Type, envelope.Payload);

        if (domainEvent is null)
        {
            _logger.LogWarning("Bilinmeyen veya deserialize edilemeyen event tipi: {Type}", envelope.Type);
            return;
        }

        var notification = _mapper.ToNotification(domainEvent);
        if (notification is null)
        {
            _logger.LogDebug("Event için notification mapper sonuç döndürmedi: {Type}", envelope.Type);
            return;
        }

        await _mediator.Publish(notification, context.CancellationToken);
        _logger.LogInformation("RabbitMQ event işlendi: {EventType}", domainEvent.GetType().Name);
    }

    private static IDomainEvent? Deserialize(string typeName, string payload)
    {
        var type = Type.GetType(typeName);
        if (type is null) return null;

        try
        {
            return JsonSerializer.Deserialize(payload, type) as IDomainEvent;
        }
        catch
        {
            return null;
        }
    }
}
