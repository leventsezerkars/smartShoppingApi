namespace smartShoppingProject.Application.Products.Commands.IncreaseProductStock;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class IncreaseProductStockCommandHandler : IRequestHandler<IncreaseProductStockCommand, Response<IncreaseProductStockResponse>>
{
    private readonly IProductRepository _productRepository;

    public IncreaseProductStockCommandHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<IncreaseProductStockResponse>> Handle(IncreaseProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Response<IncreaseProductStockResponse>.Fail("Ürün bulunamadı.");
        try
        {
            product.IncreaseStock(request.Quantity);
            return Response<IncreaseProductStockResponse>.Ok(new IncreaseProductStockResponse(product.Id, product.StockQuantity));
        }
        catch (DomainException ex)
        {
            return Response<IncreaseProductStockResponse>.Fail(ex.Message);
        }
    }
}
