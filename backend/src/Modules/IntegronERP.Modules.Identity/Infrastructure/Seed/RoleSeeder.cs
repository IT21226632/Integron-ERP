using IntegronERP.Modules.Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Infrastructure.Seed;

public static class RoleSeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>
                    {
                        Name = role
                    });
            }
        }
    }
}