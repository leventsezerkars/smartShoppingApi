namespace smartShoppingProject.Application.Categories.Commands.RenameCategory;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

public sealed record RenameCategoryCommand(Guid CategoryId, string Name) : ICommand<Response<RenameCategoryResponse>>;

public sealed record RenameCategoryResponse(Guid CategoryId);
