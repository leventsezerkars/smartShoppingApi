namespace smartShoppingProject.Application.Products.Commands.ChangeProductPrice;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record ChangeProductPriceCommand(Guid ProductId, decimal Amount, string Currency) : ICommand<Response<ChangeProductPriceResponse>>;

public sealed record ChangeProductPriceResponse(Guid ProductId);
