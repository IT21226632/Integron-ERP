using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Constants;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class TransferWarehouseStockCommandHandler
    : IRequestHandler<
        TransferWarehouseStockCommand,
        TransferWarehouseStockResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IWarehouseStockMovementRepository
        _warehouseStockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferWarehouseStockCommandHandler(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IWarehouseStockRepository warehouseStockRepository,
        IWarehouseStockMovementRepository warehouseStockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _warehouseStockRepository = warehouseStockRepository;
        _warehouseStockMovementRepository =
            warehouseStockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferWarehouseStockResponse> Handle(
        TransferWarehouseStockCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Verify product
        var product =
            await _productRepository.GetByIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Verify source warehouse
        var fromWarehouse =
            await _warehouseRepository.GetByIdAsync(
                command.Request.FromWarehouseId,
                command.CompanyId,
                cancellationToken);

        if (fromWarehouse is null)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message = "Source warehouse not found."
            };
        }

        // 3. Verify destination warehouse
        var toWarehouse =
            await _warehouseRepository.GetByIdAsync(
                command.Request.ToWarehouseId,
                command.CompanyId,
                cancellationToken);

        if (toWarehouse is null)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message = "Destination warehouse not found."
            };
        }

        // 4. Verify source warehouse is active
        if (!fromWarehouse.IsActive)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message = "Source warehouse is inactive."
            };
        }

        // 5. Verify destination warehouse is active
        if (!toWarehouse.IsActive)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message = "Destination warehouse is inactive."
            };
        }

        // 6. Get source warehouse stock
        var sourceStock =
            await _warehouseStockRepository
                .GetByProductAndWarehouseAsync(
                    command.ProductId,
                    command.Request.FromWarehouseId,
                    command.CompanyId,
                    cancellationToken);

        if (sourceStock is null)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message =
                    "Product has no stock in the source warehouse."
            };
        }

        // 7. Check available source stock
        var sourceAvailableQuantity =
            sourceStock.Quantity -
            sourceStock.ReservedQuantity;

        if (command.Request.Quantity >
            sourceAvailableQuantity)
        {
            return new TransferWarehouseStockResponse
            {
                Success = false,
                Message =
                    $"Insufficient available stock in source warehouse. " +
                    $"Available quantity: {sourceAvailableQuantity}."
            };
        }

        // 8. Get destination warehouse stock
        var destinationStock =
            await _warehouseStockRepository
                .GetByProductAndWarehouseAsync(
                    command.ProductId,
                    command.Request.ToWarehouseId,
                    command.CompanyId,
                    cancellationToken);

        // Capture quantities BEFORE modification
        var sourceQuantityBefore =
            sourceStock.Quantity;

        var destinationQuantityBefore =
            destinationStock?.Quantity ?? 0;

        // 9. Reduce source warehouse stock
        sourceStock.Quantity -=
            command.Request.Quantity;

        sourceStock.UpdatedAt =
            DateTime.UtcNow;

        await _warehouseStockRepository.UpdateAsync(
            sourceStock,
            cancellationToken);

        // 10. Create or update destination warehouse stock
        if (destinationStock is null)
        {
            destinationStock = new WarehouseStock
            {
                Id = Guid.NewGuid(),
                CompanyId = command.CompanyId,
                ProductId = command.ProductId,
                WarehouseId =
                    command.Request.ToWarehouseId,
                Quantity =
                    command.Request.Quantity,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _warehouseStockRepository.AddAsync(
                destinationStock,
                cancellationToken);
        }
        else
        {
            destinationStock.Quantity +=
                command.Request.Quantity;

            destinationStock.UpdatedAt =
                DateTime.UtcNow;

            await _warehouseStockRepository.UpdateAsync(
                destinationStock,
                cancellationToken);
        }

        // 11. Create TransferOut movement
        var transferOutMovement =
            new WarehouseStockMovement
            {
                Id = Guid.NewGuid(),
                CompanyId = command.CompanyId,
                ProductId = command.ProductId,
                WarehouseId =
                    command.Request.FromWarehouseId,

                MovementType =
                    WarehouseStockMovementType.TransferOut,

                Quantity =
                    -command.Request.Quantity,

                QuantityBefore =
                    sourceQuantityBefore,

                QuantityAfter =
                    sourceStock.Quantity,

                Reference =
                    command.Request.Reference,

                Notes =
                    command.Request.Notes,

                CreatedAt =
                    DateTime.UtcNow
            };

        await _warehouseStockMovementRepository.AddAsync(
            transferOutMovement,
            cancellationToken);

        // 12. Create TransferIn movement
        var transferInMovement =
            new WarehouseStockMovement
            {
                Id = Guid.NewGuid(),
                CompanyId = command.CompanyId,
                ProductId = command.ProductId,
                WarehouseId =
                    command.Request.ToWarehouseId,

                MovementType =
                    WarehouseStockMovementType.TransferIn,

                Quantity =
                    command.Request.Quantity,

                QuantityBefore =
                    destinationQuantityBefore,

                QuantityAfter =
                    destinationStock.Quantity,

                Reference =
                    command.Request.Reference,

                Notes =
                    command.Request.Notes,

                CreatedAt =
                    DateTime.UtcNow
            };

        await _warehouseStockMovementRepository.AddAsync(
            transferInMovement,
            cancellationToken);

        // 13. Commit all changes together
        await _unitOfWork.CommitAsync(
            cancellationToken);

        // 14. Return response
        return new TransferWarehouseStockResponse
        {
            Success = true,
            Message =
                "Stock transferred between warehouses successfully.",

            ProductId =
                command.ProductId,

            FromWarehouseId =
                command.Request.FromWarehouseId,

            ToWarehouseId =
                command.Request.ToWarehouseId,

            TransferredQuantity =
                command.Request.Quantity,

            FromWarehouseQuantity =
                sourceStock.Quantity,

            ToWarehouseQuantity =
                destinationStock.Quantity
        };
    }
}