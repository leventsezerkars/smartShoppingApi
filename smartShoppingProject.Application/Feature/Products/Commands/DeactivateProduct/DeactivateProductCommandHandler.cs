namespace smartShoppingProject.Application.Products.Commands.DeactivateProduct;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, Response<DeactivateProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public DeactivateProductCommandHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<DeactivateProductResponse>> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Response<DeactivateProductResponse>.Fail("Ürün bulunamadı.");
        product.Deactivate();
        return Response<DeactivateProductResponse>.Ok(new DeactivateProductResponse(product.Id));
    }
}
