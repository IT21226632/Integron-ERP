using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Constants;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IStockMovementRepository
{
    Task AddAsync(
        StockMovement movement,
        CancellationToken cancellationToken);

    Task<(List<StockMovement> Items, int TotalCount)> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        int page,
        int pageSize,
        StockMovementType? movementType,
        DateTime? fromDate,
        DateTime? toDate,
        CancellationToken cancellationToken);
}