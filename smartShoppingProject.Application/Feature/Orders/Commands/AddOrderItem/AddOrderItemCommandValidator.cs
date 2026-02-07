namespace smartShoppingProject.Application.Orders.Commands.AddOrderItem;

using FluentValidation;

public sealed class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
{
    public AddOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Sipariş kimliği boş olamaz.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün kimliği boş olamaz.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır.");
    }
}
