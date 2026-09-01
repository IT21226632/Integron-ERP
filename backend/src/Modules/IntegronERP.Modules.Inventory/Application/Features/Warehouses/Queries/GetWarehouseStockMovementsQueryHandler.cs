using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public class GetWarehouseStockMovementsQueryHandler
    : IRequestHandler<
        GetWarehouseStockMovementsQuery,
        GetWarehouseStockMovementsResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseStockMovementRepository
        _warehouseStockMovementRepository;

    public GetWarehouseStockMovementsQueryHandler(
        IWarehouseRepository warehouseRepository,
        IWarehouseStockMovementRepository warehouseStockMovementRepository)
    {
        _warehouseRepository = warehouseRepository;
        _warehouseStockMovementRepository =
            warehouseStockMovementRepository;
    }

    public async Task<GetWarehouseStockMovementsResponse> Handle(
        GetWarehouseStockMovementsQuery query,
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
            return new GetWarehouseStockMovementsResponse
            {
                Success = false,
                Message = "Warehouse not found.",
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        // 2. Get movements
        var result =
            await _warehouseStockMovementRepository
                .GetByWarehouseIdAsync(
                    query.WarehouseId,
                    query.CompanyId,
                    query.Page,
                    query.PageSize,
                    query.MovementType,
                    query.FromDate,
                    query.ToDate,
                    cancellationToken);

        // 3. Map movements
        var items = result.Items
            .Select(x => new WarehouseStockMovementDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                WarehouseId = x.WarehouseId,
                MovementType =
                    x.MovementType.ToString(),
                Quantity = x.Quantity,
                QuantityBefore = x.QuantityBefore,
                QuantityAfter = x.QuantityAfter,
                Reference = x.Reference,
                Notes = x.Notes,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        // 4. Calculate total pages
        var totalPages =
            (int)Math.Ceiling(
                result.TotalCount /
                (double)query.PageSize);

        return new GetWarehouseStockMovementsResponse
        {
            Success = true,
            Message =
                "Warehouse stock movements retrieved successfully.",
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = totalPages
        };
    }
}