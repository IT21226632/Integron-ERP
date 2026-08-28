using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record UpdateWarehouseStatusCommand(
    Guid WarehouseId,
    Guid CompanyId,
    UpdateWarehouseStatusRequest Request)
    : IRequest<UpdateWarehouseStatusResponse>;