using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public class GetWarehousesQueryHandler
    : IRequestHandler<
        GetWarehousesQuery,
        GetWarehousesResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehousesQueryHandler(
        IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<GetWarehousesResponse> Handle(
        GetWarehousesQuery query,
        CancellationToken cancellationToken)
    {
        var warehouses =
            await _warehouseRepository.GetByCompanyIdAsync(
                query.CompanyId,
                query.ActiveOnly,
                cancellationToken);

        var warehouseDtos = warehouses
            .Select(x => new WarehouseDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Address = x.Address,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToList();

        return new GetWarehousesResponse
        {
            Success = true,
            Message = "Warehouses retrieved successfully.",
            Warehouses = warehouseDtos
        };
    }
}