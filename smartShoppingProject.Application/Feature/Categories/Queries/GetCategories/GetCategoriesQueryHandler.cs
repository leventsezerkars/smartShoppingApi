namespace smartShoppingProject.Application.Categories.Queries.GetCategories;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedResponse<CategoryListDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<PagedResponse<CategoryListDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _categoryRepository.GetCategoriesAsync(request.PageNumber, request.PageSize, cancellationToken);
        var dtos = items.Select(c => new CategoryListDto(c.Id, c.Name, c.IsActive)).ToList();
        return PagedResponse<CategoryListDto>.Ok(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}
