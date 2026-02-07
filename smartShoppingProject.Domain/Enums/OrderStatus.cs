namespace smartShoppingProject.Domain.Enums;

/// <summary>
/// Sipariş yaşam döngüsü: Beklemede -> Ödendi -> Kargoya verildi -> Tamamlandı. İptal yalnızca kargoya verilmeden önce yapılabilir.
/// </summary>
public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Cancelled = 2,
    Shipped = 3,
    Completed = 4
}
