namespace smartShoppingProject.Application.Products.Commands.DecreaseProductStock;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record DecreaseProductStockCommand(Guid ProductId, int Quantity) : ICommand<Response<DecreaseProductStockResponse>>;

public sealed record DecreaseProductStockResponse(Guid ProductId, int NewStockQuantity);
