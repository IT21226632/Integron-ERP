using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

public class GetProductStockQueryHandler
    : IRequestHandler<
        GetProductStockQuery,
        GetProductStockResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;

    public GetProductStockQueryHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
    }

    public async Task<GetProductStockResponse> Handle(
        GetProductStockQuery query,
        CancellationToken cancellationToken)
    {
        var product =
            await _productRepository.GetByIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new GetProductStockResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        var stock =
            await _productStockRepository.GetByProductIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        if (stock is null)
        {
            return new GetProductStockResponse
            {
                Success = true,
                Message = "Product has no stock record.",
                Stock = new ProductStockDto
                {
                    ProductId = product.Id,
                    Quantity = 0,
                    ReservedQuantity = 0,
                    AvailableQuantity = 0,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        // var availableQuantity =
        //     stock.Quantity - stock.ReservedQuantity;

        return new GetProductStockResponse
        {
            Success = true,
            Message = "Stock retrieved successfully.",
            Stock = new ProductStockDto
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity,
                ReservedQuantity = stock.ReservedQuantity,
                AvailableQuantity = stock.AvailableQuantity,
                UpdatedAt = stock.UpdatedAt
            }
        };
    }
}