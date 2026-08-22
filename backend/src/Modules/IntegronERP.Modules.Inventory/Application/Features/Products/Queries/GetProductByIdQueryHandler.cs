using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductByIdResponse> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        var product =
            await _productRepository.GetByIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new GetProductByIdResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            Name = product.Name,
            SKU = product.SKU,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };

        return new GetProductByIdResponse
        {
            Success = true,
            Message = "Product retrieved successfully.",
            Product = productDto
        };
    }
}