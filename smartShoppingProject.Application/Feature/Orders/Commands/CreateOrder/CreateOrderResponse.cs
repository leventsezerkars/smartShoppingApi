namespace smartShoppingProject.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Sipariş oluşturma sonucu; minimum bilgi, veri taşıma amacı yok.
/// </summary>
public sealed record CreateOrderResponse(Guid OrderId, string Status);
