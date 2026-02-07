namespace smartShoppingProject.Application.Orders.Commands.RemoveOrderItem;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, Response<RemoveOrderItemResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<Response<RemoveOrderItemResponse>> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Response<RemoveOrderItemResponse>.Fail("Sipariş bulunamadı.");
        try
        {
            order.RemoveItem(request.OrderItemId);
            return Response<RemoveOrderItemResponse>.Ok(new RemoveOrderItemResponse(order.Id));
        }
        catch (DomainException ex)
        {
            return Response<RemoveOrderItemResponse>.Fail(ex.Message);
        }
    }
}
