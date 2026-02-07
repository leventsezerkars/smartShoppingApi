namespace smartShoppingProject.Application.Products.Queries.GetProductById;

using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<Response<ProductDto?>>;
