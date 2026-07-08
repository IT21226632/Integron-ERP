using IntegronERP.Modules.Identity.Domain.Entities;

namespace IntegronERP.Modules.Identity.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        string email,
        CancellationToken cancellationToken);

    Task AddAsync(
        Company company,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}