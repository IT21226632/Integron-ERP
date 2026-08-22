using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface IProductRepository
{
    Task AddAsync(
        Product product,
        CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken);

    Task<List<Product>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken);

    Task<bool> ExistsBySkuAsync(
        Guid companyId,
        string sku,
        CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken);

    Task<bool> ExistsBySkuAsync(
        Guid companyId,
        string sku,
        Guid excludeProductId,
        CancellationToken cancellationToken);
}