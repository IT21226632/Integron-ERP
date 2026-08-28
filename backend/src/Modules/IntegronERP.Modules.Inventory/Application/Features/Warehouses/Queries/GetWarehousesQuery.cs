using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public record GetWarehousesQuery(
    Guid CompanyId,
    bool ActiveOnly)
    : IRequest<GetWarehousesResponse>;