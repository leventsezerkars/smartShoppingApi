namespace smartShoppingProject.Application.Products.Commands.CreateProduct;

using FluentValidation;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Ürün adı zorunludur.");
        RuleFor(x => x.Description).NotNull().WithMessage("Açıklama zorunludur.");
        RuleFor(x => x.PriceAmount).GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz.");
        RuleFor(x => x.PriceCurrency).NotEmpty().WithMessage("Para birimi zorunludur.");
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");
    }
}
