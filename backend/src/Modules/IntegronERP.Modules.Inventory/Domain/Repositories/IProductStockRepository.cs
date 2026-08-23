using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IProductStockRepository
{
    Task<ProductStock?> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken);

    Task AddAsync(
        ProductStock productStock,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ProductStock productStock,
        CancellationToken cancellationToken);
}