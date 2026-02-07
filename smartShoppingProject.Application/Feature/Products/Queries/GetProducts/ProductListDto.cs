namespace smartShoppingProject.Application.Products.Queries.GetProducts;

public sealed record ProductListDto(
    Guid Id,
    string Name,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity,
    bool IsActive);
