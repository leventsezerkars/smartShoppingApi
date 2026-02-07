namespace smartShoppingProject.Application.Orders.Commands.CompleteOrder;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, Response<OrderStatusResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public CompleteOrderCommandHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<Response<OrderStatusResponse>> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Response<OrderStatusResponse>.Fail("Sipariş bulunamadı.");
        try
        {
            order.Complete();
            return Response<OrderStatusResponse>.Ok(new OrderStatusResponse(order.Id, order.Status.ToString()));
        }
        catch (DomainException ex)
        {
            return Response<OrderStatusResponse>.Fail(ex.Message);
        }
    }
}
