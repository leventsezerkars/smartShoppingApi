namespace smartShoppingProject.Application.Categories.Commands.RenameCategory;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class RenameCategoryCommandHandler : IRequestHandler<RenameCategoryCommand, Response<RenameCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public RenameCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<Response<RenameCategoryResponse>> Handle(RenameCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Response<RenameCategoryResponse>.Fail("Kategori bulunamadı.");
        try
        {
            category.Rename(request.Name);
            return Response<RenameCategoryResponse>.Ok(new RenameCategoryResponse(category.Id));
        }
        catch (DomainException ex)
        {
            return Response<RenameCategoryResponse>.Fail(ex.Message);
        }
    }
}
