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
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseStockReservationCommandHandler(
        IProductRepository productRepository,
        IProductStockRepository productStockRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReleaseStockReservationResponse> Handle(
        ReleaseStockReservationCommand command,
        CancellationToken cancellationToken)
    {
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

        stock.ReservedQuantity -=
            command.Request.Quantity;

        stock.UpdatedAt = DateTime.UtcNow;

        await _productStockRepository.UpdateAsync(
            stock,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new ReleaseStockReservationResponse
        {
            Success = true,
            Message = "Stock reservation released successfully.",
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            AvailableQuantity =
                stock.Quantity - stock.ReservedQuantity
        };
    }
}