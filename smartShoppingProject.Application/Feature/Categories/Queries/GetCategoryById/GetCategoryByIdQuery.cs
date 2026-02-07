namespace smartShoppingProject.Application.Categories.Queries.GetCategoryById;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record GetCategoryByIdQuery(Guid CategoryId) : IRequest<Response<CategoryDto?>>;
