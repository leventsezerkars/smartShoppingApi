namespace smartShoppingProject.Application.Orders.Commands.RemoveOrderItem;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record RemoveOrderItemCommand(Guid OrderId, Guid OrderItemId) : ICommand<Response<RemoveOrderItemResponse>>;

public sealed record RemoveOrderItemResponse(Guid OrderId);
