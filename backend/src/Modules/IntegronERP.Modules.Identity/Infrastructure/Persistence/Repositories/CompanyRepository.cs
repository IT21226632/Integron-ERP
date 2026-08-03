using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.Modules.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Identity.Infrastructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly IdentityDbContext _context;

    public CompanyRepository(IdentityDbContext context)
    {
        _context = context;
    }


    public async Task<Company?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }


    public async Task<bool> ExistsAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);
    }


    public async Task AddAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        await _context.Companies.AddAsync(
            company,
            cancellationToken);
    }

    public async Task<Company?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task UpdateAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        _context.Companies.Update(company);

        return Task.CompletedTask;
    }
}