namespace smartShoppingProject.Application.Events.Handlers;

using MediatR;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Application.Events.Notifications;

/// <summary>
/// ProductPriceChanged event'ini işler; iş anlamı BusinessLog'a yazılır. Kampanya, cache invalidation burada tetiklenir.
/// </summary>
public sealed class ProductPriceChangedNotificationHandler : INotificationHandler<ProductPriceChangedNotification>
{
    private readonly IBusinessLogger _businessLogger;

    public ProductPriceChangedNotificationHandler(IBusinessLogger businessLogger)
    {
        _businessLogger = businessLogger;
    }

    public Task Handle(ProductPriceChangedNotification notification, CancellationToken cancellationToken)
    {
        return _businessLogger.LogBusinessEventAsync(
            entityName: "Product",
            entityId: notification.ProductId,
            action: "ProductPriceChanged",
            context: new
            {
                OldPriceAmount = notification.OldPriceAmount,
                OldPriceCurrency = notification.OldPriceCurrency,
                NewPriceAmount = notification.NewPriceAmount,
                NewPriceCurrency = notification.NewPriceCurrency,
                OccurredOn = notification.OccurredOn
            },
            correlationId: null,
            createdAtUtc: notification.OccurredOn,
            cancellationToken);
    }
}
