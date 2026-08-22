using IntegronERP.Modules.Inventory.Domain.Entities;

namespace IntegronERP.Modules.Inventory.Domain.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken = default);

    Task<List<Category>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly = false,
        CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Category category,
        CancellationToken cancellationToken = default);

    Task<Category?> GetActiveByIdAsync(
        Guid categoryId,
        Guid companyId,
        CancellationToken cancellationToken);
}