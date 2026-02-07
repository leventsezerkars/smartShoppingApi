namespace smartShoppingProject.Application.Orders.Commands.MarkOrderAsShipped;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record MarkOrderAsShippedCommand(Guid OrderId) : ICommand<Response<OrderStatusResponse>>;

public sealed record OrderStatusResponse(Guid OrderId, string Status);
