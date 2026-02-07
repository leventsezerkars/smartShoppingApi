namespace smartShoppingProject.Application.Orders.Commands.AddOrderItem;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, Response<AddOrderItemResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Response<AddOrderItemResponse>> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            return Response<AddOrderItemResponse>.Fail("Sipariş bulunamadı.");

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Response<AddOrderItemResponse>.Fail("Ürün bulunamadı.");
        if (!product.IsActive)
            return Response<AddOrderItemResponse>.Fail("Ürün satışta değil.");

        try
        {
            order.AddItem(request.ProductId, product.Price, request.Quantity);
            return Response<AddOrderItemResponse>.Ok(new AddOrderItemResponse(order.Id));
        }
        catch (DomainException ex)
        {
            return Response<AddOrderItemResponse>.Fail(ex.Message);
        }
    }
}
