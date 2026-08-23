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
    private readonly IUnitOfWork _unitOfWork;

    public ReserveStockCommandHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReserveStockResponse> Handle(
        ReserveStockCommand command,
        CancellationToken cancellationToken)
    {
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

        if (!product.IsActive)
        {
            return new ReserveStockResponse
            {
                Success = false,
                Message = "Cannot reserve stock for an inactive product."
            };
        }

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

        var availableQuantity =
            stock.Quantity - stock.ReservedQuantity;

        if (command.Request.Quantity > availableQuantity)
        {
            return new ReserveStockResponse
            {
                Success = false,
                Message = "Insufficient available stock."
            };
        }

        stock.ReservedQuantity += command.Request.Quantity;
        stock.UpdatedAt = DateTime.UtcNow;

        await _productStockRepository.UpdateAsync(
            stock,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new ReserveStockResponse
        {
            Success = true,
            Message = "Stock reserved successfully.",
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            AvailableQuantity =
                stock.Quantity - stock.ReservedQuantity
        };
    }
}