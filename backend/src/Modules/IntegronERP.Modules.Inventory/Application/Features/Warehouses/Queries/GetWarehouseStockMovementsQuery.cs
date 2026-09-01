using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Constants;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public record GetWarehouseStockMovementsQuery(
    Guid WarehouseId,
    Guid CompanyId,
    int Page,
    int PageSize,
    WarehouseStockMovementType? MovementType,
    DateTime? FromDate,
    DateTime? ToDate)
    : IRequest<GetWarehouseStockMovementsResponse>;