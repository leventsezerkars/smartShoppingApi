namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Common;
using smartShoppingProject.Domain.Enums;
using smartShoppingProject.Domain.Events;
using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;

/// <summary>
/// Sipariş aggregate'ı; kalem yönetimi ve durum geçişleriyle invariant'ları korur.
/// </summary>
public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();
    private Currency _currency = Currency.TRY;

    /// <summary>
    /// EF Core materialization için; iş kuralları için kullanılmaz.
    /// </summary>
    private Order() { }

    /// <summary>
    /// Persistence tarafından yükleme için para birimi; iş kuralları _currency kullanır.
    /// </summary>
    public Currency Currency { get => _currency; private set => _currency = value; }

    private Order(Guid id, Guid customerId)
        : base(id)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException("Müşteri kimliği boş olamaz.");
        }

        CustomerId = customerId;
        Status = OrderStatus.Pending;
        TotalAmount = new Money(0, _currency);
    }

    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money TotalAmount { get; private set; }

    public static Order Create(
        Guid customerId,
        IEnumerable<(Guid ProductId, Money UnitPrice, int Quantity)> items)
    {
        // Kural: Sipariş anlamlı olması için en az bir kalem içermelidir.
        if (items is null)
        {
            throw new InvalidOrderItemException("Siparişte en az bir kalem bulunmalıdır.");
        }

        var itemList = items.ToList();
        if (itemList.Count == 0)
        {
            throw new InvalidOrderItemException("Siparişte en az bir kalem bulunmalıdır.");
        }

        var order = new Order(Guid.NewGuid(), customerId);

        foreach (var item in itemList)
        {
            order.AddItem(item.ProductId, item.UnitPrice, item.Quantity);
        }

        order.AddDomainEvent(new OrderCreatedEvent(order.Id, DateTime.UtcNow));
        return order;
    }

    public void AddItem(Guid productId, Money unitPrice, int quantity)
    {
        // Kural: Geçerli ara toplam için miktar pozitif olmalıdır.
        if (quantity <= 0)
        {
            throw new InvalidOrderItemException("Miktar sıfırdan büyük olmalıdır.");
        }

        // Kural: Birim fiyat geçerli bir Money değeri olmalıdır (primitive kullanılmaz).
        if (unitPrice is null)
        {
            throw new InvalidOrderItemException("Birim fiyat zorunludur.");
        }

        // Kural: Her kalem gerçek bir ürünü referans etmelidir.
        if (productId == Guid.Empty)
        {
            throw new InvalidOrderItemException("Ürün kimliği boş olamaz.");
        }

        // Kural: Karışık para birimleri toplamı bozar; siparişte tek para birimi zorunludur.
        if (_items.Count == 0)
        {
            _currency = unitPrice.Currency;
        }
        else if (_currency != unitPrice.Currency)
        {
            throw new InvalidOrderItemException("Tüm sipariş kalemleri aynı para biriminde olmalıdır.");
        }

        _items.Add(new OrderItem(Guid.NewGuid(), productId, unitPrice, quantity));
        RecalculateTotal();
        SetUpdatedAt();
    }

    public void RemoveItem(Guid orderItemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == orderItemId);

        // Kural: Olmayan kalemin silinmesi hatalı bir iş akışına işaret eder.
        if (item is null)
        {
            throw new InvalidOrderItemException($"Sipariş kalemi bulunamadı: '{orderItemId}'.");
        }

        // Kural: Siparişte en az bir kalem kalmalıdır; boş sipariş aggregate amacına aykırıdır.
        if (_items.Count == 1)
        {
            throw new InvalidOrderItemException("Siparişte en az bir kalem bulunmalıdır.");
        }

        _items.Remove(item);
        RecalculateTotal();
        SetUpdatedAt();
    }

    public void MarkAsPaid()
    {
        // Kural: Yaşam döngüsü tutarlılığı için yalnızca bekleyen siparişler ödenebilir.
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOrderStateException("Yalnızca bekleyen siparişler ödenmiş olarak işaretlenebilir.");
        }

        Status = OrderStatus.Paid;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        // Kural: Teslim bütünlüğü için kargoya verilmiş veya tamamlanmış siparişler iptal edilemez.
        if (Status == OrderStatus.Shipped || Status == OrderStatus.Completed)
        {
            throw new InvalidOrderStateException("Kargoya verilmiş veya tamamlanmış siparişler iptal edilemez.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            return;
        }

        Status = OrderStatus.Cancelled;
        SetUpdatedAt();
        AddDomainEvent(new OrderCancelledEvent(Id, DateTime.UtcNow));
    }

    public void MarkAsShipped()
    {
        // Kural: Kargoya verme yalnızca ödemeden sonra yapılabilir.
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOrderStateException("Yalnızca ödenmiş siparişler kargoya verilebilir.");
        }

        Status = OrderStatus.Shipped;
        SetUpdatedAt();
    }

    public void Complete()
    {
        // Kural: Tamamlanma yalnızca kargo onayından sonra yapılabilir.
        if (Status != OrderStatus.Shipped)
        {
            throw new InvalidOrderStateException("Yalnızca kargoya verilmiş siparişler tamamlanabilir.");
        }

        Status = OrderStatus.Completed;
        SetUpdatedAt();
    }

    private void RecalculateTotal()
    {
        if (_items.Count == 0)
        {
            TotalAmount = new Money(0, _currency);
            return;
        }

        Money total = new(0, _currency);
        foreach (var item in _items)
        {
            total += item.TotalPrice;
        }

        TotalAmount = total;
    }
}
