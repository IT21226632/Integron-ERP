using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IStockMovementRepository
{
    Task AddAsync(
        StockMovement movement,
        CancellationToken cancellationToken);

    Task<List<StockMovement>> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken);
}