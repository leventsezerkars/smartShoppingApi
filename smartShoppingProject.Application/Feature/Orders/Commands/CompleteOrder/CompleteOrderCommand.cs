namespace smartShoppingProject.Application.Orders.Commands.CompleteOrder;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record CompleteOrderCommand(Guid OrderId) : ICommand<Response<OrderStatusResponse>>;

public sealed record OrderStatusResponse(Guid OrderId, string Status);
