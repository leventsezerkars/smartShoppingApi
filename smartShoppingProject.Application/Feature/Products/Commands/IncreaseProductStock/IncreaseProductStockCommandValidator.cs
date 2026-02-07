namespace smartShoppingProject.Application.Products.Commands.IncreaseProductStock;

using FluentValidation;

public sealed class IncreaseProductStockCommandValidator : AbstractValidator<IncreaseProductStockCommand>
{
    public IncreaseProductStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün kimliği boş olamaz.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır.");
    }
}
