using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class CreateWarehouseCommandHandler
    : IRequestHandler<
        CreateWarehouseCommand,
        CreateWarehouseResponse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseCommandHandler(
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateWarehouseResponse> Handle(
        CreateWarehouseCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Request.Name.Trim();
        var code = command.Request.Code.Trim().ToUpperInvariant();

        var exists =
            await _warehouseRepository.ExistsByCodeAsync(
                command.CompanyId,
                code,
                cancellationToken);

        if (exists)
        {
            return new CreateWarehouseResponse
            {
                Success = false,
                Message = "A warehouse with this code already exists."
            };
        }

        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            Name = name,
            Code = code,
            Address = string.IsNullOrWhiteSpace(
                command.Request.Address)
                ? null
                : command.Request.Address.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _warehouseRepository.AddAsync(
            warehouse,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new CreateWarehouseResponse
        {
            Success = true,
            Message = "Warehouse created successfully.",
            WarehouseId = warehouse.Id
        };
    }
}