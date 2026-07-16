using IntegronERP.Modules.Identity.Domain.Entities;

namespace IntegronERP.Modules.Identity.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);
}