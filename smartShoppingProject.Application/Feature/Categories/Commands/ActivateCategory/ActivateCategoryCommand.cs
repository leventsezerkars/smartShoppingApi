namespace smartShoppingProject.Application.Categories.Commands.ActivateCategory;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record ActivateCategoryCommand(Guid CategoryId) : ICommand<Response<ActivateCategoryResponse>>;

public sealed record ActivateCategoryResponse(Guid CategoryId);
