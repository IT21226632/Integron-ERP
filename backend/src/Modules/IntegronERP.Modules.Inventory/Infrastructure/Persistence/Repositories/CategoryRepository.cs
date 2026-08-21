using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly InventoryDbContext _context;

    public CategoryRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(
            category,
            cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(
                x => x.CompanyId == companyId &&
                     x.Name == name,
                cancellationToken);
    }

    public async Task<List<Category>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .Where(x => x.CompanyId == companyId);

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(
                x => x.Id == id &&
                    x.CompanyId == companyId,
                cancellationToken);
    }

    public Task UpdateAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);

        return Task.CompletedTask;
    }
}