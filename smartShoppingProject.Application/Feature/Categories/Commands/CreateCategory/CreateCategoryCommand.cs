namespace smartShoppingProject.Application.Categories.Commands.CreateCategory;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record CreateCategoryCommand(string Name) : ICommand<Response<CreateCategoryResponse>>;

public sealed record CreateCategoryResponse(Guid CategoryId, string Name);
