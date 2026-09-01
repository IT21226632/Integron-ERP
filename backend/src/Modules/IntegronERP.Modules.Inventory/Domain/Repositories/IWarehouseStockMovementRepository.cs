using IntegronERP.Modules.Inventory.Domain.Constants;
using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IWarehouseStockMovementRepository
{
    Task AddAsync(
        WarehouseStockMovement movement,
        CancellationToken cancellationToken);

    Task<(List<WarehouseStockMovement> Items, int TotalCount)> GetByWarehouseIdAsync(
            Guid warehouseId,
            Guid companyId,
            int page,
            int pageSize,
            WarehouseStockMovementType? movementType,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken);
}