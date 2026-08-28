using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IWarehouseRepository
{
    Task AddAsync(
        Warehouse warehouse,
        CancellationToken cancellationToken);

    Task<Warehouse?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken);

    Task<List<Warehouse>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid companyId,
        string code,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Warehouse warehouse,
        CancellationToken cancellationToken);
}