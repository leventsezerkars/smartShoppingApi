namespace smartShoppingProject.Application.Products.Queries.GetProducts;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<ProductListDto>>;
