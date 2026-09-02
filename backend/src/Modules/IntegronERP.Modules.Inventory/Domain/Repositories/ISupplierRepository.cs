using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface ISupplierRepository
{
    Task AddAsync(
        Supplier supplier,
        CancellationToken cancellationToken);

    Task<Supplier?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken);

    Task<List<Supplier>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        Guid excludeSupplierId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Supplier supplier,
        CancellationToken cancellationToken);
}