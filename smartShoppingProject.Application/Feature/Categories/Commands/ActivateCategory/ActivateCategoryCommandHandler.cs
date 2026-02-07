namespace smartShoppingProject.Application.Categories.Commands.ActivateCategory;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class ActivateCategoryCommandHandler : IRequestHandler<ActivateCategoryCommand, Response<ActivateCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public ActivateCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<Response<ActivateCategoryResponse>> Handle(ActivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Response<ActivateCategoryResponse>.Fail("Kategori bulunamadı.");
        category.Activate();
        return Response<ActivateCategoryResponse>.Ok(new ActivateCategoryResponse(category.Id));
    }
}
