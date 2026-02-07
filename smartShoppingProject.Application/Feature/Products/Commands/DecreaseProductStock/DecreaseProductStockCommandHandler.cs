namespace smartShoppingProject.Application.Products.Commands.DecreaseProductStock;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class DecreaseProductStockCommandHandler : IRequestHandler<DecreaseProductStockCommand, Response<DecreaseProductStockResponse>>
{
    private readonly IProductRepository _productRepository;

    public DecreaseProductStockCommandHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<DecreaseProductStockResponse>> Handle(DecreaseProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Response<DecreaseProductStockResponse>.Fail("Ürün bulunamadı.");
        try
        {
            product.DecreaseStock(request.Quantity);
            return Response<DecreaseProductStockResponse>.Ok(new DecreaseProductStockResponse(product.Id, product.StockQuantity));
        }
        catch (DomainException ex)
        {
            return Response<DecreaseProductStockResponse>.Fail(ex.Message);
        }
    }
}
