namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;

/// <summary>
/// OrderItem yalnızca Order aggregate'ı içinde yaşar; fiyat ve miktar kurallarını korur.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// Aggregate içi ve persistence materialization için.
    /// </summary>
    public OrderItem(Guid id, Guid productId, Money unitPrice, int quantity)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidOrderItemException("Sipariş kalemi kimliği boş olamaz.");
        }

        if (productId == Guid.Empty)
        {
            throw new InvalidOrderItemException("Ürün kimliği boş olamaz.");
        }

        if (unitPrice is null)
        {
            throw new InvalidOrderItemException("Birim fiyat zorunludur.");
        }

        if (quantity <= 0)
        {
            throw new InvalidOrderItemException("Miktar sıfırdan büyük olmalıdır.");
        }

        Id = id;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid Id { get; }
    public Guid ProductId { get; }
    public Money UnitPrice { get; }
    public int Quantity { get; }

    public Money TotalPrice => UnitPrice * Quantity;
}
