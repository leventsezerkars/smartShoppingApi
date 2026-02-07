namespace smartShoppingProject.Application.Products.Queries.GetProductById;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<ProductDto?>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<ProductDto?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var readModel = await _productRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
        if (readModel is null)
            return Response<ProductDto?>.Ok(null);
        var dto = new ProductDto(
            readModel.Id,
            readModel.Name,
            readModel.Description,
            readModel.PriceAmount,
            readModel.PriceCurrency,
            readModel.StockQuantity,
            readModel.IsActive,
            readModel.CreatedAt);
        return Response<ProductDto?>.Ok(dto);
    }
}
