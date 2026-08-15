using IntegronERP.Modules.Inventory.Infrastructure.Persistence;
using IntegronERP.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;

namespace IntegronERP.Modules.Inventory;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(
                    "DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, InventoryUnitOfWork>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}