using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

public class GetWarehouseByIdQueryHandler
    : IRequestHandler<
        GetWarehouseByIdQuery,
        GetWarehouseByIdResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehouseByIdQueryHandler(
        IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<GetWarehouseByIdResponse> Handle(
        GetWarehouseByIdQuery query,
        CancellationToken cancellationToken)
    {
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                query.WarehouseId,
                query.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new GetWarehouseByIdResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        return new GetWarehouseByIdResponse
        {
            Success = true,
            Message = "Warehouse retrieved successfully.",
            Warehouse = new WarehouseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Code = warehouse.Code,
                Address = warehouse.Address,
                IsActive = warehouse.IsActive,
                CreatedAt = warehouse.CreatedAt,
                UpdatedAt = warehouse.UpdatedAt
            }
        };
    }
}