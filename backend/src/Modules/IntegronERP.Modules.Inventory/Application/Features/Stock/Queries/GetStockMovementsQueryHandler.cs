using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

public class GetStockMovementsQueryHandler
    : IRequestHandler<
        GetStockMovementsQuery,
        GetStockMovementsResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;

    public GetStockMovementsQueryHandler(
        IProductRepository productRepository,
        IStockMovementRepository stockMovementRepository)
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<GetStockMovementsResponse> Handle(
        GetStockMovementsQuery query,
        CancellationToken cancellationToken)
    {
        var product =
            await _productRepository.GetByIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new GetStockMovementsResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        var movements =
            await _stockMovementRepository.GetByProductIdAsync(
                query.ProductId,
                query.CompanyId,
                cancellationToken);

        var movementDtos = movements
            .Select(x => new StockMovementDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                MovementType = x.MovementType.ToString(),
                Quantity = x.Quantity,
                QuantityBefore = x.QuantityBefore,
                QuantityAfter = x.QuantityAfter,
                Reference = x.Reference,
                Notes = x.Notes,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return new GetStockMovementsResponse
        {
            Success = true,
            Message = "Stock movements retrieved successfully.",
            Movements = movementDtos
        };
    }
}