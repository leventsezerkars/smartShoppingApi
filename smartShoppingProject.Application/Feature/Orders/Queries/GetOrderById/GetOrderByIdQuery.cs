namespace smartShoppingProject.Application.Orders.Queries.GetOrderById;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

/// <summary>
/// Sipariş detayı okuma sorgusu. Sadece okuma; state değiştirmez.
/// </summary>
public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Response<OrderDto?>>;
