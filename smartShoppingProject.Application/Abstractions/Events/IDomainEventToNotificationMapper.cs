namespace smartShoppingProject.Application.Abstractions.Events;

using MediatR;
using smartShoppingProject.Domain.Events;

/// <summary>
/// Domain event'i MediatR notification'a çevirir; InMemoryEventBus ve RabbitMQ consumer bu sayede aynı handler'ları kullanır.
/// </summary>
public interface IDomainEventToNotificationMapper
{
    /// <summary>
    /// Bilinen bir domain event ise ilgili INotification örneğini döner; değilse null.
    /// </summary>
    INotification? ToNotification(IDomainEvent domainEvent);
}
