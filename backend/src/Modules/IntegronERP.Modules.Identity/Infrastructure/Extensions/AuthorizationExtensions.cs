using IntegronERP.Modules.Identity.Infrastructure.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IntegronERP.Modules.Identity.Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddIdentityAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policies.ManageUsers,
                policy =>
                    policy.RequireRole("Owner"));

            options.AddPolicy(
                Policies.ManageRoles,
                policy =>
                    policy.RequireRole("Owner"));

            options.AddPolicy(
                Policies.ManageCompany,
                policy =>
                    policy.RequireRole("Owner"));
        });

        return services;
    }
}