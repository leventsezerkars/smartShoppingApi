namespace smartShoppingProject.Application.Orders.Commands.AddOrderItem;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record AddOrderItemCommand(Guid OrderId, Guid ProductId, int Quantity) : ICommand<Response<AddOrderItemResponse>>;

public sealed record AddOrderItemResponse(Guid OrderId);
