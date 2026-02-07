namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Common;
using smartShoppingProject.Domain.Events;
using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;

public sealed class Product : AggregateRoot
{
    public Product(
        Guid id,
        string name,
        string description,
        Money price,
        int stockQuantity,
        bool isActive = true)
        : base(id)
    {
        Name = EnsureName(name);
        Description = EnsureDescription(description);
        Price = EnsurePrice(price);
        StockQuantity = EnsureStock(stockQuantity);
        IsActive = isActive;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    public void ChangePrice(Money newPrice)
    {
        newPrice = EnsurePrice(newPrice);

        if (newPrice.Equals(Price))
        {
            return;
        }

        var oldPrice = Price;
        Price = newPrice;
        SetUpdatedAt();
        AddDomainEvent(new ProductPriceChangedEvent(Id, oldPrice, newPrice));
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Stok artış miktarı pozitif olmalıdır.");
        }

        StockQuantity += quantity;
        SetUpdatedAt();
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Stok düşüm miktarı pozitif olmalıdır.");
        }

        if (StockQuantity - quantity < 0)
        {
            throw new InsufficientStockException(Id, StockQuantity, quantity);
        }

        StockQuantity -= quantity;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        SetUpdatedAt();
    }

    private static string EnsureName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Ürün adı zorunludur.");
        }

        return name.Trim();
    }

    private static string EnsureDescription(string description)
    {
        if (description is null)
        {
            throw new DomainException("Ürün açıklaması zorunludur.");
        }

        return description.Trim();
    }

    private static Money EnsurePrice(Money price)
    {
        if (price is null)
        {
            throw new InvalidPriceException("Fiyat zorunludur.");
        }

        return price;
    }

    private static int EnsureStock(int stockQuantity)
    {
        if (stockQuantity < 0)
        {
            throw new DomainException("Stok miktarı negatif olamaz.");
        }

        return stockQuantity;
    }
}
