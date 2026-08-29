using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public class ReserveStockCommandHandler
    : IRequestHandler<
        ReserveStockCommand,
        ReserveStockResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveStockCommandHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IWarehouseStockRepository warehouseStockRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _warehouseStockRepository = warehouseStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReserveStockResponse> Handle(
        ReserveStockCommand command,
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
            return new ReserveStockResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Product must be active
        if (!product.IsActive)
        {
            return new ReserveStockResponse
            {
                Success = false,
                Message =
                    "Cannot reserve stock for an inactive product."
            };
        }

        // 3. Get product stock
        var stock =
            await _productStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (stock is null)
        {
            return new ReserveStockResponse
            {
                Success = false,
                Message = "Product has no stock available."
            };
        }

        // 4. Get stock allocated to warehouses
        var warehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        // 5. Calculate total allocated stock
        var allocatedQuantity =
            warehouseStocks.Sum(x => x.Quantity);

        // 6. Calculate stock available for reservation
        var availableQuantity =
            Math.Max(
                0,
                stock.Quantity
                - allocatedQuantity
                - stock.ReservedQuantity);

        // 7. Validate requested reservation
        if (command.Request.Quantity > availableQuantity)
        {
            return new ReserveStockResponse
            {
                Success = false,
                Message =
                    $"Insufficient available stock. " +
                    $"Available stock for reservation: " +
                    $"{availableQuantity}."
            };
        }

        // 8. Reserve stock
        stock.ReservedQuantity +=
            command.Request.Quantity;

        stock.UpdatedAt =
            DateTime.UtcNow;

        await _productStockRepository.UpdateAsync(
            stock,
            cancellationToken);

        // 9. Commit
        await _unitOfWork.CommitAsync(
            cancellationToken);

        // 10. Calculate remaining available stock
        var newAvailableQuantity =
            Math.Max(
                0,
                stock.Quantity
                - allocatedQuantity
                - stock.ReservedQuantity);

        // 11. Return response
        return new ReserveStockResponse
        {
            Success = true,
            Message = "Stock reserved successfully.",
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            AvailableQuantity = newAvailableQuantity
        };
    }
}