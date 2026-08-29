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
    private readonly IWarehouseStockRepository _warehouseStockRepository;

    public GetProductStockQueryHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IWarehouseStockRepository warehouseStockRepository)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _warehouseStockRepository = warehouseStockRepository;
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
                    AllocatedQuantity = 0,
                    UnallocatedQuantity = 0,
                    ReservedQuantity = 0,
                    AvailableQuantity = 0,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        // Get stock allocated to all warehouses
        var warehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        // Total stock currently allocated to warehouses
        var allocatedQuantity =
            warehouseStocks.Sum(x => x.Quantity);

        // Stock that has not yet been allocated to a warehouse
        var unallocatedQuantity =
            Math.Max(
                0,
                stock.Quantity - allocatedQuantity);

        // Stock available after considering both
        // warehouse allocation and reservations
        var availableQuantity =
            Math.Max(
                0,
                stock.Quantity -
                allocatedQuantity -
                stock.ReservedQuantity);

        return new GetProductStockResponse
        {
            Success = true,
            Message = "Stock retrieved successfully.",
            Stock = new ProductStockDto
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity,
                AllocatedQuantity = allocatedQuantity,
                UnallocatedQuantity = unallocatedQuantity,
                ReservedQuantity = stock.ReservedQuantity,
                AvailableQuantity = availableQuantity,
                UpdatedAt = stock.UpdatedAt
            }
        };
    }
}