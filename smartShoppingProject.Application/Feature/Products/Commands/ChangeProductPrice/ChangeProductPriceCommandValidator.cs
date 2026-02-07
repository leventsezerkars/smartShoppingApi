namespace smartShoppingProject.Application.Products.Commands.ChangeProductPrice;

using FluentValidation;

public sealed class ChangeProductPriceCommandValidator : AbstractValidator<ChangeProductPriceCommand>
{
    public ChangeProductPriceCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün kimliği boş olamaz.");
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz.");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Para birimi zorunludur.");
    }
}
