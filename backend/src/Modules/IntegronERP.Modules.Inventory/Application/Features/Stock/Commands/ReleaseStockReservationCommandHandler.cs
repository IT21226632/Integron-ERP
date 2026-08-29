using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public class ReleaseStockReservationCommandHandler
    : IRequestHandler<
        ReleaseStockReservationCommand,
        ReleaseStockReservationResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IWarehouseStockRepository _warehouseStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseStockReservationCommandHandler(
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

    public async Task<ReleaseStockReservationResponse> Handle(
        ReleaseStockReservationCommand command,
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
            return new ReleaseStockReservationResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Get product stock
        var stock =
            await _productStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (stock is null)
        {
            return new ReleaseStockReservationResponse
            {
                Success = false,
                Message = "Product has no stock record."
            };
        }

        // 3. Validate reserved quantity
        if (command.Request.Quantity >
            stock.ReservedQuantity)
        {
            return new ReleaseStockReservationResponse
            {
                Success = false,
                Message =
                    "Insufficient reserved stock to release."
            };
        }

        // 4. Get warehouse allocations
        var warehouseStocks =
            await _warehouseStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        // 5. Calculate total allocated stock
        var allocatedQuantity =
            warehouseStocks.Sum(x => x.Quantity);

        // 6. Release reservation
        stock.ReservedQuantity -=
            command.Request.Quantity;

        stock.UpdatedAt =
            DateTime.UtcNow;

        await _productStockRepository.UpdateAsync(
            stock,
            cancellationToken);

        // 7. Commit
        await _unitOfWork.CommitAsync(
            cancellationToken);

        // 8. Calculate available stock after release
        var availableQuantity =
            Math.Max(
                0,
                stock.Quantity
                - allocatedQuantity
                - stock.ReservedQuantity);

        // 9. Return response
        return new ReleaseStockReservationResponse
        {
            Success = true,
            Message =
                "Stock reservation released successfully.",
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            AvailableQuantity = availableQuantity
        };
    }
}