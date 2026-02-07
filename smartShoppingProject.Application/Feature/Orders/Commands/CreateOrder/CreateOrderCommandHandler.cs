namespace smartShoppingProject.Application.Orders.Commands.CreateOrder;

using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;
using MediatR;

/// <summary>
/// Sipariş oluşturma use-case orkestrasyonu. Response&lt;T&gt; döner; domain/infrastructure detayı sızdırmaz.
/// Başarılı oluşturma business log'a yazılır (IBusinessLogger); teknik hatalar ILogger ile Serilog'a gider.
/// </summary>
public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreateOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBusinessLogger _businessLogger;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IBusinessLogger businessLogger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _businessLogger = businessLogger;
    }

    public async Task<Response<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var itemsWithPrice = new List<(Guid ProductId, Money UnitPrice, int Quantity)>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null)
            {
                return Response<CreateOrderResponse>.Fail($"Ürün bulunamadı: {item.ProductId}");
            }
            if (!product.IsActive)
            {
                return Response<CreateOrderResponse>.Fail($"Ürün satışta değil: {item.ProductId}");
            }

            itemsWithPrice.Add((item.ProductId, product.Price, item.Quantity));
        }

        try
        {
            var order = Order.Create(request.CustomerId, itemsWithPrice);
            _orderRepository.Add(order);

            await _businessLogger.LogBusinessEventAsync(
                entityName: "Order",
                entityId: order.Id,
                action: "OrderCreated",
                context: new { CustomerId = request.CustomerId, ItemCount = request.Items.Count },
                correlationId: null,
                createdAtUtc: DateTime.UtcNow,
                cancellationToken);

            return Response<CreateOrderResponse>.Ok(new CreateOrderResponse(order.Id, order.Status.ToString()));
        }
        catch (DomainException ex)
        {
            return Response<CreateOrderResponse>.Fail(ex.Message);
        }
    }
}
