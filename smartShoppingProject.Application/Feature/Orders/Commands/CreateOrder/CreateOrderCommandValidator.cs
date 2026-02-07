namespace smartShoppingProject.Application.Orders.Commands.CreateOrder;

using FluentValidation;

/// <summary>
/// CreateOrder komutu için doğrulama. Application katmanında; domain kuralları validator'a taşınmaz.
/// </summary>
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Müşteri kimliği boş olamaz.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Siparişte en az bir kalem bulunmalıdır.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Ürün kimliği boş olamaz.");
            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Miktar sıfırdan büyük olmalıdır.");
        });
    }
}
