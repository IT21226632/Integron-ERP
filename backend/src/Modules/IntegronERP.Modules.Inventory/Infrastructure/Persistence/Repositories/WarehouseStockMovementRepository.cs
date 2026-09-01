using IntegronERP.Modules.Inventory.Domain.Constants;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseStockMovementRepository
    : IWarehouseStockMovementRepository
{
    private readonly InventoryDbContext _context;

    public WarehouseStockMovementRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        WarehouseStockMovement movement,
        CancellationToken cancellationToken)
    {
        await _context.WarehouseStockMovements.AddAsync(
            movement,
            cancellationToken);
    }

    public async Task<(
        List<WarehouseStockMovement> Items,
        int TotalCount)>
        GetByWarehouseIdAsync(
            Guid warehouseId,
            Guid companyId,
            int page,
            int pageSize,
            WarehouseStockMovementType? movementType,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken)
    {
        var query = _context.WarehouseStockMovements
            .AsNoTracking()
            .Where(x =>
                x.WarehouseId == warehouseId &&
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

        var totalCount =
            await query.CountAsync(cancellationToken);

        var items =
            await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}