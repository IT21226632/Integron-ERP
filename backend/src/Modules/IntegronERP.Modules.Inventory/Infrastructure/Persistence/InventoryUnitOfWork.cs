using IntegronERP.SharedKernel.Interfaces;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence;

public class InventoryUnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;

    public InventoryUnitOfWork(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(
            cancellationToken);
    }
}