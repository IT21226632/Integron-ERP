using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Queries;

public class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetCategoriesResponse> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories =
            await _categoryRepository.GetByCompanyIdAsync(
                query.CompanyId,
                query.ActiveOnly,
                cancellationToken);

        var categoryDtos = categories
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return new GetCategoriesResponse
        {
            Success = true,
            Message = "Categories retrieved successfully.",
            Categories = categoryDtos
        };
    }
}