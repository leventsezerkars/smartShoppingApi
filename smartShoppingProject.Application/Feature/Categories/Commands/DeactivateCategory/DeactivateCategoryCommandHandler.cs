namespace smartShoppingProject.Application.Categories.Commands.DeactivateCategory;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class DeactivateCategoryCommandHandler : IRequestHandler<DeactivateCategoryCommand, Response<DeactivateCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<Response<DeactivateCategoryResponse>> Handle(DeactivateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Response<DeactivateCategoryResponse>.Fail("Kategori bulunamadı.");
        category.Deactivate();
        return Response<DeactivateCategoryResponse>.Ok(new DeactivateCategoryResponse(category.Id));
    }
}
