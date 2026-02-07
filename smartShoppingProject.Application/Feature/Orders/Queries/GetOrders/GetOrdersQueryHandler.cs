namespace smartShoppingProject.Application.Orders.Queries.GetOrders;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResponse<OrderListDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<PagedResponse<OrderListDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _orderRepository.GetOrdersAsync(request.PageNumber, request.PageSize, cancellationToken);
        var dtos = items.Select(o => new OrderListDto(o.Id, o.CustomerId, o.Status, o.TotalAmount, o.Currency, o.CreatedAt)).ToList();
        return PagedResponse<OrderListDto>.Ok(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}
