using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class UpdateWarehouseStatusCommandHandler
    : IRequestHandler<
        UpdateWarehouseStatusCommand,
        UpdateWarehouseStatusResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWarehouseStatusCommandHandler(
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateWarehouseStatusResponse> Handle(
        UpdateWarehouseStatusCommand command,
        CancellationToken cancellationToken)
    {
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                command.WarehouseId,
                command.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new UpdateWarehouseStatusResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        warehouse.IsActive = command.Request.IsActive;
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _warehouseRepository.UpdateAsync(
            warehouse,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateWarehouseStatusResponse
        {
            Success = true,
            Message = command.Request.IsActive
                ? "Warehouse activated successfully."
                : "Warehouse deactivated successfully.",
            WarehouseId = warehouse.Id,
            IsActive = warehouse.IsActive
        };
    }
}