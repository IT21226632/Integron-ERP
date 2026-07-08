using IntegronERP.Modules.Identity.Domain.Entities;

namespace IntegronERP.Modules.Identity.Domain.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);
}