using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryDbContext _context;

    public SupplierRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Supplier supplier,
        CancellationToken cancellationToken)
    {
        await _context.Suppliers.AddAsync(
            supplier,
            cancellationToken);
    }

    public async Task<Supplier?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(
                x =>
                    x.Id == id &&
                    x.CompanyId == companyId,
                cancellationToken);
    }

    public async Task<List<Supplier>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken)
    {
        var query = _context.Suppliers
            .Where(x => x.CompanyId == companyId);

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .AnyAsync(
                x =>
                    x.CompanyId == companyId &&
                    x.Name == name,
                cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        Guid companyId,
        string name,
        Guid excludeSupplierId,
        CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .AnyAsync(
                x =>
                    x.CompanyId == companyId &&
                    x.Name == name &&
                    x.Id != excludeSupplierId,
                cancellationToken);
    }

    public Task UpdateAsync(
        Supplier supplier,
        CancellationToken cancellationToken)
    {
        _context.Suppliers.Update(supplier);

        return Task.CompletedTask;
    }
}