namespace smartShoppingProject.Application.Orders.Queries.GetOrderById;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

/// <summary>
/// Sipariş okuma use-case. Response&lt;OrderDto?&gt; döner; domain entity sızdırılmaz.
/// </summary>
public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<OrderDto?>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Response<OrderDto?>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var readModel = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
        if (readModel is null)
        {
            return Response<OrderDto?>.Ok(null);
        }

        var items = readModel.Items
            .Select(i => new OrderItemDto(i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.TotalPrice))
            .ToList();

        var dto = new OrderDto(
            readModel.Id,
            readModel.CustomerId,
            readModel.Status,
            readModel.TotalAmount,
            readModel.Currency,
            readModel.CreatedAt,
            items);

        return Response<OrderDto?>.Ok(dto);
    }
}
