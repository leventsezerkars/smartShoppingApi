namespace smartShoppingProject.Application.Products.Commands.ChangeProductPrice;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Enums;
using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;
using MediatR;

public sealed class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand, Response<ChangeProductPriceResponse>>
{
    private readonly IProductRepository _productRepository;

    public ChangeProductPriceCommandHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<ChangeProductPriceResponse>> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Response<ChangeProductPriceResponse>.Fail("Ürün bulunamadı.");
        if (!Enum.TryParse<Currency>(request.Currency, ignoreCase: true, out var currency))
            return Response<ChangeProductPriceResponse>.Fail("Geçersiz para birimi.");
        try
        {
            product.ChangePrice(new Money(request.Amount, currency));
            return Response<ChangeProductPriceResponse>.Ok(new ChangeProductPriceResponse(product.Id));
        }
        catch (DomainException ex)
        {
            return Response<ChangeProductPriceResponse>.Fail(ex.Message);
        }
    }
}
