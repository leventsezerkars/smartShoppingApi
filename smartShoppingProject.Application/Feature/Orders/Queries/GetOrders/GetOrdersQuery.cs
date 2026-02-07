namespace smartShoppingProject.Application.Orders.Queries.GetOrders;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record GetOrdersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<OrderListDto>>;
