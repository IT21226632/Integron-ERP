using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Constants;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class ReturnWarehouseStockCommandHandler
    : IRequestHandler<
        ReturnWarehouseStockCommand,
        ReturnWarehouseStockResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IWarehouseStockMovementRepository
        _warehouseStockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturnWarehouseStockCommandHandler(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IProductStockRepository productStockRepository,
        IWarehouseStockRepository warehouseStockRepository,
        IWarehouseStockMovementRepository
            warehouseStockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _productStockRepository = productStockRepository;
        _warehouseStockRepository = warehouseStockRepository;
        _warehouseStockMovementRepository =
            warehouseStockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReturnWarehouseStockResponse> Handle(
        ReturnWarehouseStockCommand command,
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
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Verify warehouse
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                command.WarehouseId,
                command.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        // 3. Warehouse must be active
        if (!warehouse.IsActive)
        {
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message = "Warehouse is inactive."
            };
        }

        // 4. Get main product stock
        var productStock =
            await _productStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (productStock is null)
        {
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message = "Product has no stock record."
            };
        }

        // 5. Get warehouse stock
        var warehouseStock =
            await _warehouseStockRepository
                .GetByProductAndWarehouseAsync(
                    command.ProductId,
                    command.WarehouseId,
                    command.CompanyId,
                    cancellationToken);

        if (warehouseStock is null)
        {
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message =
                    "Product has no stock in this warehouse."
            };
        }

        // 6. Calculate available warehouse stock
        var warehouseAvailableQuantity =
            warehouseStock.Quantity -
            warehouseStock.ReservedQuantity;

        // 7. Check requested quantity
        if (command.Request.Quantity >
            warehouseAvailableQuantity)
        {
            return new ReturnWarehouseStockResponse
            {
                Success = false,
                Message =
                    $"Insufficient available stock in warehouse. " +
                    $"Available quantity: " +
                    $"{warehouseAvailableQuantity}."
            };
        }

        // 8. Capture quantity before change
        var warehouseQuantityBefore =
            warehouseStock.Quantity;

        // 9. Remove stock from warehouse
        warehouseStock.Quantity -=
            command.Request.Quantity;

        warehouseStock.UpdatedAt =
            DateTime.UtcNow;

        await _warehouseStockRepository.UpdateAsync(
            warehouseStock,
            cancellationToken);

        // 10. Recalculate total allocated stock
        //
        // ProductStock.Quantity remains unchanged.
        // AllocatedQuantity is calculated from warehouse stocks.
        var warehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        var allocatedQuantity =
            warehouseStocks.Sum(x => x.Quantity);

        // 11. Calculate unallocated stock
        var unallocatedQuantity =
            productStock.Quantity -
            allocatedQuantity;

        // 12. Create warehouse movement
        var warehouseMovement =
            new WarehouseStockMovement
            {
                Id = Guid.NewGuid(),

                CompanyId =
                    command.CompanyId,

                ProductId =
                    command.ProductId,

                WarehouseId =
                    command.WarehouseId,

                MovementType =
                    WarehouseStockMovementType.TransferOut,

                Quantity =
                    command.Request.Quantity,

                QuantityBefore =
                    warehouseQuantityBefore,

                QuantityAfter =
                    warehouseStock.Quantity,

                Reference =
                    command.Request.Reference,

                Notes =
                    command.Request.Notes,

                CreatedAt =
                    DateTime.UtcNow
            };

        await _warehouseStockMovementRepository.AddAsync(
            warehouseMovement,
            cancellationToken);

        // 13. Commit warehouse stock + movement together
        await _unitOfWork.CommitAsync(
            cancellationToken);

        // 14. Return result
        return new ReturnWarehouseStockResponse
        {
            Success = true,

            Message =
                "Stock returned to main product stock successfully.",

            ProductId =
                command.ProductId,

            WarehouseId =
                command.WarehouseId,

            ReturnedQuantity =
                command.Request.Quantity,

            WarehouseQuantity =
                warehouseStock.Quantity,

            WarehouseAvailableQuantity =
                warehouseStock.AvailableQuantity,

            TotalProductStock =
                productStock.Quantity,

            AllocatedQuantity =
                allocatedQuantity,

            UnallocatedQuantity =
                unallocatedQuantity
        };
    }
}