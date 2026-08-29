using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseStockRepository
    : IWarehouseStockRepository
{
    private readonly InventoryDbContext _context;

    public WarehouseStockRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<WarehouseStock?> GetByProductAndWarehouseAsync(
        Guid productId,
        Guid warehouseId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.WarehouseStocks
            .FirstOrDefaultAsync(
                x =>
                    x.ProductId == productId &&
                    x.WarehouseId == warehouseId &&
                    x.CompanyId == companyId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<WarehouseStock>> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.WarehouseStocks
            .Where(
                x =>
                    x.ProductId == productId &&
                    x.CompanyId == companyId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        WarehouseStock warehouseStock,
        CancellationToken cancellationToken)
    {
        await _context.WarehouseStocks.AddAsync(
            warehouseStock,
            cancellationToken);
    }

    public Task UpdateAsync(
        WarehouseStock warehouseStock,
        CancellationToken cancellationToken)
    {
        _context.WarehouseStocks.Update(warehouseStock);

        return Task.CompletedTask;
    }
}