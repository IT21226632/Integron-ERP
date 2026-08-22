using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.Id == id &&
                     x.CompanyId == companyId,
                cancellationToken);
    }

    public async Task<List<Product>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(x => x.Category)
            .Where(x => x.CompanyId == companyId);

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySkuAsync(
        Guid companyId,
        string sku,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(
                x => x.CompanyId == companyId &&
                     x.SKU == sku,
                cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(
                x => x.CompanyId == companyId &&
                     x.Name == name,
                cancellationToken);
    }

    public Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        _context.Products.Update(product);

        return Task.CompletedTask;
    }
}