using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class UpdateWarehouseCommandHandler
    : IRequestHandler<
        UpdateWarehouseCommand,
        UpdateWarehouseResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWarehouseCommandHandler(
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateWarehouseResponse> Handle(
        UpdateWarehouseCommand command,
        CancellationToken cancellationToken)
    {
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                command.WarehouseId,
                command.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new UpdateWarehouseResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        var name = command.Request.Name.Trim();

        var code =
            command.Request.Code
                .Trim()
                .ToUpperInvariant();

        var codeChanged =
            !string.Equals(
                warehouse.Code,
                code,
                StringComparison.OrdinalIgnoreCase);

        if (codeChanged)
        {
            var codeExists =
                await _warehouseRepository.ExistsByCodeAsync(
                    command.CompanyId,
                    code,
                    cancellationToken);

            if (codeExists)
            {
                return new UpdateWarehouseResponse
                {
                    Success = false,
                    Message =
                        "A warehouse with this code already exists."
                };
            }
        }

        warehouse.Name = name;
        warehouse.Code = code;
        warehouse.Address =
            string.IsNullOrWhiteSpace(
                command.Request.Address)
                ? null
                : command.Request.Address.Trim();

        warehouse.UpdatedAt = DateTime.UtcNow;

        await _warehouseRepository.UpdateAsync(
            warehouse,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateWarehouseResponse
        {
            Success = true,
            Message = "Warehouse updated successfully.",
            WarehouseId = warehouse.Id
        };
    }
}