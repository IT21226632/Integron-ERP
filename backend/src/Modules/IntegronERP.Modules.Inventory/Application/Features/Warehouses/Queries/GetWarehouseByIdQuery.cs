using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public record GetWarehouseByIdQuery(
    Guid WarehouseId,
    Guid CompanyId)
    : IRequest<GetWarehouseByIdResponse>;