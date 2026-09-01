using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record ReturnWarehouseStockCommand(
    Guid ProductId,
    Guid CompanyId,
    Guid WarehouseId,
    ReturnWarehouseStockRequest Request)
    : IRequest<ReturnWarehouseStockResponse>;