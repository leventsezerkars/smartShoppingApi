namespace smartShoppingProject.Application.Products.Commands.IncreaseProductStock;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record IncreaseProductStockCommand(Guid ProductId, int Quantity) : ICommand<Response<IncreaseProductStockResponse>>;

public sealed record IncreaseProductStockResponse(Guid ProductId, int NewStockQuantity);
