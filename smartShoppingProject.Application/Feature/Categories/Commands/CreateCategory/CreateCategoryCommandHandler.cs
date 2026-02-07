namespace smartShoppingProject.Application.Categories.Commands.CreateCategory;

using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Domain.Exceptions;
using MediatR;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<CreateCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<Response<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new Category(Guid.NewGuid(), request.Name);
            _categoryRepository.Add(category);
            return Response<CreateCategoryResponse>.Ok(new CreateCategoryResponse(category.Id, category.Name));
        }
        catch (DomainException ex)
        {
            return Response<CreateCategoryResponse>.Fail(ex.Message);
        }
    }
}
