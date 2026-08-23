using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Constants;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public class StockAdjustmentCommandHandler
    : IRequestHandler<
        StockAdjustmentCommand,
        StockAdjustmentResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StockAdjustmentCommandHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockAdjustmentResponse> Handle(
        StockAdjustmentCommand command,
        CancellationToken cancellationToken)
    {
        var product =
            await _productRepository.GetByIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new StockAdjustmentResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        if (!product.IsActive)
        {
            return new StockAdjustmentResponse
            {
                Success = false,
                Message = "Cannot adjust stock for an inactive product."
            };
        }

        var stock =
            await _productStockRepository.GetByProductIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        var quantityBefore = stock?.Quantity ?? 0;

        var quantityAfter =
            quantityBefore + command.Request.Quantity;

        if (quantityAfter < 0)
        {
            return new StockAdjustmentResponse
            {
                Success = false,
                Message = "Stock quantity cannot be negative."
            };
        }

        if (stock is null)
        {
            stock = new ProductStock
            {
                Id = Guid.NewGuid(),
                CompanyId = command.CompanyId,
                ProductId = command.ProductId,
                Quantity = quantityAfter,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productStockRepository.AddAsync(
                stock,
                cancellationToken);
        }
        else
        {
            stock.Quantity = quantityAfter;
            stock.UpdatedAt = DateTime.UtcNow;

            await _productStockRepository.UpdateAsync(
                stock,
                cancellationToken);
        }

        var movement = new StockMovement
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            ProductId = command.ProductId,
            MovementType = StockMovementType.Adjustment,
            Quantity = command.Request.Quantity,
            QuantityBefore = quantityBefore,
            QuantityAfter = quantityAfter,
            Reference = command.Request.Reference,
            Notes = command.Request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _stockMovementRepository.AddAsync(
            movement,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new StockAdjustmentResponse
        {
            Success = true,
            Message = "Stock adjusted successfully.",
            ProductId = product.Id,
            QuantityBefore = quantityBefore,
            AdjustmentQuantity = command.Request.Quantity,
            QuantityAfter = quantityAfter
        };
    }
}