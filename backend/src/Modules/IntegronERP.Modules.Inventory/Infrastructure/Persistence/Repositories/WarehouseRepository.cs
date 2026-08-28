using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly InventoryDbContext _context;

    public WarehouseRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Warehouse warehouse,
        CancellationToken cancellationToken)
    {
        await _context.Warehouses.AddAsync(
            warehouse,
            cancellationToken);
    }

    public async Task<Warehouse?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(
                x =>
                    x.Id == id &&
                    x.CompanyId == companyId,
                cancellationToken);
    }

    public async Task<List<Warehouse>> GetByCompanyIdAsync(
        Guid companyId,
        bool activeOnly,
        CancellationToken cancellationToken)
    {
        var query = _context.Warehouses
            .Where(x => x.CompanyId == companyId);

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid companyId,
        string code,
        CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .AnyAsync(
                x =>
                    x.CompanyId == companyId &&
                    x.Code == code,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Warehouse warehouse,
        CancellationToken cancellationToken)
    {
        _context.Warehouses.Update(warehouse);

        await Task.CompletedTask;
    }
}