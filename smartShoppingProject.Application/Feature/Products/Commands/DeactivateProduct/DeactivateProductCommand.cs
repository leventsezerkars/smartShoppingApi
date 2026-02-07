namespace smartShoppingProject.Application.Products.Commands.DeactivateProduct;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record DeactivateProductCommand(Guid ProductId) : ICommand<Response<DeactivateProductResponse>>;

public sealed record DeactivateProductResponse(Guid ProductId);
