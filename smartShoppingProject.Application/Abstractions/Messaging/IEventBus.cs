namespace smartShoppingProject.Application.Abstractions.Messaging;

using smartShoppingProject.Domain.Events;

/// <summary>
/// Domain event'lerin yayımlanması için sözleşme. Implementation Infrastructure'da (ör. MediatR, message queue).
/// </summary>
public interface IEventBus
{
    Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
