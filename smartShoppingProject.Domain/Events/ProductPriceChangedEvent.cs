namespace smartShoppingProject.Domain.Events;

using smartShoppingProject.Domain.ValueObjects;

public sealed class ProductPriceChangedEvent : IDomainEvent
{
    public ProductPriceChangedEvent(Guid productId, Money oldPrice, Money newPrice)
    {
        ProductId = productId;
        OldPrice = oldPrice ?? throw new ArgumentNullException(nameof(oldPrice));
        NewPrice = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
        OccurredOn = DateTime.UtcNow;
    }

    public Guid ProductId { get; }
    public Money OldPrice { get; }
    public Money NewPrice { get; }
    public DateTime OccurredOn { get; }
}
