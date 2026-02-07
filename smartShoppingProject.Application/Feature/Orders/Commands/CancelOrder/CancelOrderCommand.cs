namespace smartShoppingProject.Application.Orders.Commands.CancelOrder;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record CancelOrderCommand(Guid OrderId) : ICommand<Response<OrderStatusResponse>>;

public sealed record OrderStatusResponse(Guid OrderId, string Status);
