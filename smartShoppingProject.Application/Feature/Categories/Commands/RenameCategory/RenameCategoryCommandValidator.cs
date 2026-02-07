namespace smartShoppingProject.Application.Categories.Commands.RenameCategory;

using FluentValidation;

public sealed class RenameCategoryCommandValidator : AbstractValidator<RenameCategoryCommand>
{
    public RenameCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori kimliği boş olamaz.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Kategori adı zorunludur.");
    }
}
