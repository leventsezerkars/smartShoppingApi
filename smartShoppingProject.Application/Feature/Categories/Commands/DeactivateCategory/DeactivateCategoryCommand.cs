namespace smartShoppingProject.Application.Categories.Commands.DeactivateCategory;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record DeactivateCategoryCommand(Guid CategoryId) : ICommand<Response<DeactivateCategoryResponse>>;

public sealed record DeactivateCategoryResponse(Guid CategoryId);
