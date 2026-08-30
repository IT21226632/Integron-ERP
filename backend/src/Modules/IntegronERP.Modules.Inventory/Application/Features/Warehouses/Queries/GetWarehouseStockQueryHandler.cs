using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public class GetWarehouseStockQueryHandler
    : IRequestHandler<
        GetWarehouseStockQuery,
        GetWarehouseStockResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;

    public GetWarehouseStockQueryHandler(
        IWarehouseRepository warehouseRepository,
        IWarehouseStockRepository warehouseStockRepository)
    {
        _warehouseRepository = warehouseRepository;
        _warehouseStockRepository = warehouseStockRepository;
    }

    public async Task<GetWarehouseStockResponse> Handle(
        GetWarehouseStockQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Verify warehouse
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                query.WarehouseId,
                query.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new GetWarehouseStockResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        // 2. Get warehouse stock
        var warehouseStocks =
            await _warehouseStockRepository.GetByWarehouseIdAsync(
                query.WarehouseId,
                query.CompanyId,
                cancellationToken);

        // 3. Map to DTOs
        var stockDtos =
            warehouseStocks
                .Select(x => new WarehouseStockDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    ReservedQuantity = x.ReservedQuantity,
                    AvailableQuantity = x.AvailableQuantity,
                    UpdatedAt = x.UpdatedAt
                })
                .ToList();

        return new GetWarehouseStockResponse
        {
            Success = true,
            Message = "Warehouse stock retrieved successfully.",
            WarehouseId = warehouse.Id,
            WarehouseName = warehouse.Name,
            Stocks = stockDtos
        };
    }
}