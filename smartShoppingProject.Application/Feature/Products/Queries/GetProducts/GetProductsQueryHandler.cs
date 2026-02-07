namespace smartShoppingProject.Application.Products.Queries.GetProducts;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResponse<ProductListDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<PagedResponse<ProductListDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _productRepository.GetProductsAsync(request.PageNumber, request.PageSize, cancellationToken);
        var dtos = items.Select(p => new ProductListDto(p.Id, p.Name, p.PriceAmount, p.PriceCurrency, p.StockQuantity, p.IsActive)).ToList();
        return PagedResponse<ProductListDto>.Ok(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}
