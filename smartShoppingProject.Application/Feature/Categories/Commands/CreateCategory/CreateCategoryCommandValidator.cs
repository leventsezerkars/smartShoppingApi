namespace smartShoppingProject.Application.Categories.Commands.CreateCategory;

using FluentValidation;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Kategori adı zorunludur.");
    }
}
