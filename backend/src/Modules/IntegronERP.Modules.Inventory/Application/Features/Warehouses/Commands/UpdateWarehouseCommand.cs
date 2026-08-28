using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record UpdateWarehouseCommand(
    Guid WarehouseId,
    Guid CompanyId,
    UpdateWarehouseRequest Request)
    : IRequest<UpdateWarehouseResponse>;