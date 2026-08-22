using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Queries;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, GetProductsResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductsResponse> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products =
            await _productRepository.GetByCompanyIdAsync(
                query.CompanyId,
                query.ActiveOnly,
                cancellationToken);

        var productDtos = products
            .Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                CategoryName = x.Category?.Name ?? string.Empty,
                Name = x.Name,
                SKU = x.SKU,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return new GetProductsResponse
        {
            Success = true,
            Message = "Products retrieved successfully.",
            Products = productDtos
        };
    }
}