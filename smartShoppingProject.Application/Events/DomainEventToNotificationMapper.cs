namespace smartShoppingProject.Application.Events;

using MediatR;
using smartShoppingProject.Application.Abstractions.Events;
using smartShoppingProject.Application.Events.Notifications;
using smartShoppingProject.Domain.Events;

/// <summary>
/// Tüm domain event tiplerini ilgili MediatR notification'a map eder.
/// </summary>
public sealed class DomainEventToNotificationMapper : IDomainEventToNotificationMapper
{
    public INotification? ToNotification(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            OrderCreatedEvent e => new OrderCreatedNotification(e.OrderId, e.OccurredOn),
            OrderCancelledEvent e => new OrderCancelledNotification(e.OrderId, e.OccurredOn),
            ProductPriceChangedEvent e => new ProductPriceChangedNotification(
                e.ProductId,
                e.OldPrice.Amount,
                e.OldPrice.Currency.ToString(),
                e.NewPrice.Amount,
                e.NewPrice.Currency.ToString(),
                e.OccurredOn),
            _ => null
        };
    }
}
