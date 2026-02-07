namespace smartShoppingProject.Infrastructure.Messaging;

using MediatR;
using Microsoft.Extensions.Logging;
using smartShoppingProject.Application.Abstractions.Events;
using smartShoppingProject.Application.Abstractions.Messaging;
using smartShoppingProject.Domain.Events;

/// <summary>
/// Domain event'leri MediatR notification'a map edip handler'lara dağıtır. In-memory tek process içinde çalışır.
/// </summary>
internal sealed class InMemoryEventBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly IDomainEventToNotificationMapper _mapper;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(
        IMediator mediator,
        IDomainEventToNotificationMapper mapper,
        ILogger<InMemoryEventBus> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (domainEvents.Count == 0) return;

        foreach (var evt in domainEvents)
        {
            var notification = _mapper.ToNotification(evt);
            if (notification is not null)
            {
                await _mediator.Publish(notification, cancellationToken);
                _logger.LogDebug("Event handler'lara gönderildi: {EventType}", evt.GetType().Name);
            }
        }
    }
}
