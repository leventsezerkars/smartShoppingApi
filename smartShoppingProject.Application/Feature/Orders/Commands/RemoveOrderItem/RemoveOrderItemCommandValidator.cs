namespace smartShoppingProject.Application.Orders.Commands.RemoveOrderItem;

using FluentValidation;

public sealed class RemoveOrderItemCommandValidator : AbstractValidator<RemoveOrderItemCommand>
{
    public RemoveOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Sipariş kimliği boş olamaz.");
        RuleFor(x => x.OrderItemId).NotEmpty().WithMessage("Sipariş kalemi kimliği boş olamaz.");
    }
}
