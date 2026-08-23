using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using IntegronERP.Modules.Inventory.Domain.Constants;

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

    public async Task<(List<StockMovement> Items, int TotalCount)> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        int page,
        int pageSize,
        StockMovementType? movementType,
        DateTime? fromDate,
        DateTime? toDate,
        CancellationToken cancellationToken)
    {
        var query = _context.StockMovements
            .AsNoTracking()
            .Where(x =>
                x.ProductId == productId &&
                x.CompanyId == companyId);

        if (movementType.HasValue)
        {
            query = query.Where(
                x => x.MovementType == movementType.Value);
        }

        if (fromDate.HasValue)
        {
            var startDate = DateTime.SpecifyKind(
                fromDate.Value.Date,
                DateTimeKind.Utc);

            query = query.Where(
                x => x.CreatedAt >= startDate);
        }

        if (toDate.HasValue)
        {
            var endDate = DateTime.SpecifyKind(
                toDate.Value.Date.AddDays(1),
                DateTimeKind.Utc);

            query = query.Where(
                x => x.CreatedAt < endDate);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}