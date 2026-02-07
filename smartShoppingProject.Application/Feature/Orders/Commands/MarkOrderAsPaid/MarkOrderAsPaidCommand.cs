namespace smartShoppingProject.Application.Orders.Commands.MarkOrderAsPaid;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record MarkOrderAsPaidCommand(Guid OrderId) : ICommand<Response<OrderStatusResponse>>;

public sealed record OrderStatusResponse(Guid OrderId, string Status);
