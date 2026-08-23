using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly InventoryDbContext _context;

    public StockMovementRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        StockMovement movement,
        CancellationToken cancellationToken)
    {
        await _context.StockMovements.AddAsync(
            movement,
            cancellationToken);
    }

    public async Task<List<StockMovement>> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.StockMovements
            .Where(x =>
                x.ProductId == productId &&
                x.CompanyId == companyId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}