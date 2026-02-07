namespace smartShoppingProject.Application.Abstractions.Repositories;

using smartShoppingProject.Domain.Entities;

/// <summary>
/// Ürün aggregate'ı için persistence sözleşmesi. Implementation Infrastructure katmanındadır.
/// </summary>
public interface IProductRepository
{
    void Add(Product product);
    Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductReadModel?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ProductReadModel> Items, int TotalCount)> GetProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}

/// <summary>
/// Ürün okuma modeli; query tarafında kullanılır.
/// </summary>
public sealed record ProductReadModel(
    Guid Id,
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAt);
