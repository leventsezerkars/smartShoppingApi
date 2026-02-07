namespace smartShoppingProject.Application.Products.Commands.CreateProduct;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Domain.Enums;
using smartShoppingProject.Domain.Exceptions;
using smartShoppingProject.Domain.ValueObjects;
using MediatR;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<Response<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Currency>(request.PriceCurrency, ignoreCase: true, out var currency))
            return Response<CreateProductResponse>.Fail("Geçersiz para birimi.");

        try
        {
            var price = new Money(request.PriceAmount, currency);
            var product = new Product(Guid.NewGuid(), request.Name, request.Description, price, request.StockQuantity);
            _productRepository.Add(product);
            return Response<CreateProductResponse>.Ok(new CreateProductResponse(product.Id, product.Name));
        }
        catch (DomainException ex)
        {
            return Response<CreateProductResponse>.Fail(ex.Message);
        }
    }
}
