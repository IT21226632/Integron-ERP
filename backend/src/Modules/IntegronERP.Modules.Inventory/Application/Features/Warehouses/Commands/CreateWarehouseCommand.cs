using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record CreateWarehouseCommand(
    CreateWarehouseRequest Request,
    Guid CompanyId)
    : IRequest<CreateWarehouseResponse>;