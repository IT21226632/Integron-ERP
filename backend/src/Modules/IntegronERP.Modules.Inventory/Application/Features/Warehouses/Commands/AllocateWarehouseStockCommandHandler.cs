using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public class AllocateWarehouseStockCommandHandler
    : IRequestHandler<
        AllocateWarehouseStockCommand,
        AllocateWarehouseStockResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AllocateWarehouseStockCommandHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IWarehouseRepository warehouseRepository,
        IWarehouseStockRepository warehouseStockRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _warehouseRepository = warehouseRepository;
        _warehouseStockRepository = warehouseStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocateWarehouseStockResponse> Handle(
        AllocateWarehouseStockCommand command,
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
            return new AllocateWarehouseStockResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Verify warehouse
        var warehouse =
            await _warehouseRepository.GetByIdAsync(
                command.Request.WarehouseId,
                command.CompanyId,
                cancellationToken);

        if (warehouse is null)
        {
            return new AllocateWarehouseStockResponse
            {
                Success = false,
                Message = "Warehouse not found."
            };
        }

        // 3. Warehouse must be active
        if (!warehouse.IsActive)
        {
            return new AllocateWarehouseStockResponse
            {
                Success = false,
                Message = "Warehouse is inactive."
            };
        }

        // 4. Get product stock
        var productStock =
            await _productStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (productStock is null)
        {
            return new AllocateWarehouseStockResponse
            {
                Success = false,
                Message = "Product has no stock record."
            };
        }

        // 5. Get existing warehouse stock
        var warehouseStock =
            await _warehouseStockRepository
                .GetByProductAndWarehouseAsync(
                    command.ProductId,
                    command.Request.WarehouseId,
                    command.CompanyId,
                    cancellationToken);

        // 6. Get stock already allocated to all warehouses
        var warehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        var totalAllocated =
            warehouseStocks.Sum(x => x.Quantity);

        // 7. Calculate unallocated stock
        var unallocatedStock =
        Math.Max(
            0,
            productStock.Quantity -
            productStock.ReservedQuantity -
            totalAllocated);

        if (command.Request.Quantity > unallocatedStock)
        {
            return new AllocateWarehouseStockResponse
            {
                Success = false,
                Message =
                    $"Insufficient unallocated stock. " +
                    $"Available stock for allocation: {unallocatedStock}."
            };
        }

        // 8. Create or update warehouse stock
        if (warehouseStock is null)
        {
            warehouseStock = new WarehouseStock
            {
                Id = Guid.NewGuid(),
                CompanyId = command.CompanyId,
                ProductId = command.ProductId,
                WarehouseId = command.Request.WarehouseId,
                Quantity = command.Request.Quantity,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _warehouseStockRepository.AddAsync(
                warehouseStock,
                cancellationToken);
        }
        else
        {
            warehouseStock.Quantity +=
                command.Request.Quantity;

            warehouseStock.UpdatedAt =
                DateTime.UtcNow;

            await _warehouseStockRepository.UpdateAsync(
                warehouseStock,
                cancellationToken);
        }

        // 9. Commit changes
        await _unitOfWork.CommitAsync(
            cancellationToken);

        // 10. Recalculate allocation after the update
        var updatedWarehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        var updatedAllocatedQuantity =
            updatedWarehouseStocks.Sum(x => x.Quantity);

        var updatedUnallocatedQuantity =
            Math.Max(
                0,
                productStock.Quantity -
                updatedAllocatedQuantity);

        // 11. Return result
        return new AllocateWarehouseStockResponse
        {
            Success = true,
            Message = "Stock allocated to warehouse successfully.",

            ProductId = command.ProductId,

            WarehouseId =
                command.Request.WarehouseId,

            WarehouseQuantity =
                warehouseStock.Quantity,

            WarehouseAvailableQuantity =
                warehouseStock.AvailableQuantity,

            TotalProductStock =
                productStock.Quantity,

            AllocatedQuantity =
                updatedAllocatedQuantity,

            UnallocatedQuantity =
                updatedUnallocatedQuantity
        };
    }
}