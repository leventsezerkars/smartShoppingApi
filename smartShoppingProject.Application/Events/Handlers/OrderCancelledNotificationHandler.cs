namespace smartShoppingProject.Application.Events.Handlers;

using MediatR;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Application.Events.Notifications;

/// <summary>
/// OrderCancelled event'ini işler; iş anlamı BusinessLog'a yazılır. İade, stok geri alma, bildirim burada tetiklenir.
/// </summary>
public sealed class OrderCancelledNotificationHandler : INotificationHandler<OrderCancelledNotification>
{
    private readonly IBusinessLogger _businessLogger;

    public OrderCancelledNotificationHandler(IBusinessLogger businessLogger)
    {
        _businessLogger = businessLogger;
    }

    public Task Handle(OrderCancelledNotification notification, CancellationToken cancellationToken)
    {
        return _businessLogger.LogBusinessEventAsync(
            entityName: "Order",
            entityId: notification.OrderId,
            action: "OrderCancelled",
            context: new { OccurredOn = notification.OccurredOn },
            correlationId: null,
            createdAtUtc: notification.OccurredOn,
            cancellationToken);
    }
}
