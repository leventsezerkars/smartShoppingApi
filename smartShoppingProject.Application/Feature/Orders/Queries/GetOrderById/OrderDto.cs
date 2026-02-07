namespace smartShoppingProject.Application.Orders.Queries.GetOrderById;

/// <summary>
/// Sipariş okuma modeli; query response için. Domain entity döndürülmez.
/// </summary>
public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt,
    IReadOnlyList<OrderItemDto> Items);

/// <summary>
/// Sipariş kalemi okuma modeli.
/// </summary>
public sealed record OrderItemDto(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);
