namespace smartShoppingProject.Application.Categories.Queries.GetCategories;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record GetCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<CategoryListDto>>;
