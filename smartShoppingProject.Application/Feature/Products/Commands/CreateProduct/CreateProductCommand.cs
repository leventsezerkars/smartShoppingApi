namespace smartShoppingProject.Application.Products.Commands.CreateProduct;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity) : ICommand<Response<CreateProductResponse>>;

public sealed record CreateProductResponse(Guid ProductId, string Name);
