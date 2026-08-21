using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Queries;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetCategoryByIdResponse> Handle(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(
            query.Id,
            query.CompanyId,
            cancellationToken);

        if (category == null)
        {
            return new GetCategoryByIdResponse
            {
                Success = false,
                Message = "Category not found."
            };
        }

        return new GetCategoryByIdResponse
        {
            Success = true,
            Message = "Category retrieved successfully.",
            Category = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            }
        };
    }
}