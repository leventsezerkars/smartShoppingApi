namespace smartShoppingProject.Application.Orders.Queries.GetOrders;

/// <summary>
/// Sipariş liste satırı; sayfalı listede kullanılır.
/// </summary>
public sealed record OrderListDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt);
