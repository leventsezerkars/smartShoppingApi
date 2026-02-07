namespace smartShoppingProject.Application.Products.Commands.DecreaseProductStock;

using FluentValidation;

public sealed class DecreaseProductStockCommandValidator : AbstractValidator<DecreaseProductStockCommand>
{
    public DecreaseProductStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün kimliği boş olamaz.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır.");
    }
}
