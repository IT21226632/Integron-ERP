using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;

    public RefreshTokenRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);
    }

    public Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }
}