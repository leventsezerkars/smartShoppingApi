namespace smartShoppingProject.Application.Events.Handlers;

using MediatR;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Application.Events.Notifications;

/// <summary>
/// OrderCreated event'ini işler; iş anlamı BusinessLog'a yazılır (Serilog değil). Bildirim/entegrasyon burada tetiklenir.
/// </summary>
public sealed class OrderCreatedNotificationHandler : INotificationHandler<OrderCreatedNotification>
{
    private readonly IBusinessLogger _businessLogger;

    public OrderCreatedNotificationHandler(IBusinessLogger businessLogger)
    {
        _businessLogger = businessLogger;
    }

    public Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        return _businessLogger.LogBusinessEventAsync(
            entityName: "Order",
            entityId: notification.OrderId,
            action: "OrderCreated",
            context: new { OccurredOn = notification.OccurredOn },
            correlationId: null,
            createdAtUtc: notification.OccurredOn,
            cancellationToken);
    }
}
