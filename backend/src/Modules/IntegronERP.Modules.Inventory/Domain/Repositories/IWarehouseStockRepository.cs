using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IWarehouseStockRepository
{
    Task<WarehouseStock?> GetByProductAndWarehouseAsync(
        Guid productId,
        Guid warehouseId,
        Guid companyId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<WarehouseStock>> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken);

    Task AddAsync(
        WarehouseStock warehouseStock,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        WarehouseStock warehouseStock,
        CancellationToken cancellationToken);
}