namespace smartShoppingProject.Application.Categories.Queries.GetCategoryById;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Response<CategoryDto?>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<Response<CategoryDto?>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var readModel = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (readModel is null)
            return Response<CategoryDto?>.Ok(null);
        return Response<CategoryDto?>.Ok(new CategoryDto(readModel.Id, readModel.Name, readModel.IsActive, readModel.CreatedAt));
    }
}
