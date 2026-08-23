using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

public class ProductStockRepository : IProductStockRepository
{
    private readonly InventoryDbContext _context;

    public ProductStockRepository(
        InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<ProductStock?> GetByProductIdAsync(
        Guid productId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.ProductStocks
            .FirstOrDefaultAsync(
                x => x.ProductId == productId &&
                     x.CompanyId == companyId,
                cancellationToken);
    }

    public async Task AddAsync(
        ProductStock productStock,
        CancellationToken cancellationToken)
    {
        await _context.ProductStocks.AddAsync(
            productStock,
            cancellationToken);
    }

    public Task UpdateAsync(
        ProductStock productStock,
        CancellationToken cancellationToken)
    {
        _context.ProductStocks.Update(productStock);

        return Task.CompletedTask;
    }
}