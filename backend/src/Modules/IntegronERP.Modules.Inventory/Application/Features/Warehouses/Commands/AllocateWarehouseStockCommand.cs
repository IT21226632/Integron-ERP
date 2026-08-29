using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record AllocateWarehouseStockCommand(
    Guid ProductId,
    Guid CompanyId,
    AllocateWarehouseStockRequest Request)
    : IRequest<AllocateWarehouseStockResponse>;