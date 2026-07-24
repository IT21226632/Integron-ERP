using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.Modules.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegronERP.Modules.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task<List<ApplicationUser>> GetByCompanyAsync(
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(x => x.CompanyId == companyId)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task UpdateAsync(
        ApplicationUser user,
        CancellationToken cancellationToken)
    {
        _context.Users.Update(user);

        return Task.CompletedTask;
    }

    public async Task<int> GetActiveOwnerCountAsync(
        Guid companyId,
        CancellationToken cancellationToken)
    {
        var owners = await (
            from user in _context.Users
            join userRole in _context.UserRoles
                on user.Id equals userRole.UserId
            join role in _context.Roles
                on userRole.RoleId equals role.Id
            where user.CompanyId == companyId
                && user.IsActive
                && role.Name == "Owner"
            select user.Id
        ).CountAsync(cancellationToken);

        return owners;
    }
}