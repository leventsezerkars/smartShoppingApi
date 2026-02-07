namespace smartShoppingProject.Application.Abstractions.Repositories;

using smartShoppingProject.Domain.Entities;

/// <summary>
/// Sipariş aggregate'ı için persistence sözleşmesi. Implementation Infrastructure katmanındadır.
/// </summary>
public interface IOrderRepository
{
    void Add(Order order);

    /// <summary>
    /// Sipariş aggregate'ını command handler'lar için getirir (state değişikliği için).
    /// </summary>
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Siparişi okuma modeli olarak getirir. Query tarafında kullanılır; domain entity döndürmez.
    /// </summary>
    Task<OrderReadModel?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sayfalı sipariş listesi; query tarafında kullanılır.
    /// </summary>
    Task<(IReadOnlyList<OrderReadModel> Items, int TotalCount)> GetOrdersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}

/// <summary>
/// Sipariş okuma modeli; repository sözleşmesi için. Handler tarafında gerekirse DTO'ya map edilir.
/// </summary>
public sealed record OrderReadModel(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt,
    IReadOnlyList<OrderItemReadModel> Items);

/// <summary>
/// Sipariş kalemi okuma modeli.
/// </summary>
public sealed record OrderItemReadModel(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);
