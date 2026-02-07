namespace smartShoppingProject.Application.Categories.Queries.GetCategoryById;

public sealed record CategoryDto(Guid Id, string Name, bool IsActive, DateTime CreatedAt);
