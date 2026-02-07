namespace smartShoppingProject.Application.Products.Queries.GetProductById;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAt);
