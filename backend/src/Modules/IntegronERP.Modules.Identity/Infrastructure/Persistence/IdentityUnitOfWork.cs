using IntegronERP.Modules.Identity.Infrastructure.Persistence;
using IntegronERP.SharedKernel.Interfaces;

namespace IntegronERP.Modules.Identity.Infrastructure.Persistence;

public class IdentityUnitOfWork : IUnitOfWork
{
    private readonly IdentityDbContext _dbContext;

    public IdentityUnitOfWork(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}