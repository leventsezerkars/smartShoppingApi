namespace smartShoppingProject.Application.Orders.Commands.MarkOrderAsPaid;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, Response<OrderStatusResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<Response<OrderStatusResponse>> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Response<OrderStatusResponse>.Fail("Sipariş bulunamadı.");
        try
        {
            order.MarkAsPaid();
            return Response<OrderStatusResponse>.Ok(new OrderStatusResponse(order.Id, order.Status.ToString()));
        }
        catch (DomainException ex)
        {
            return Response<OrderStatusResponse>.Fail(ex.Message);
        }
    }
}
