using IntegronERP.Modules.Inventory.Infrastructure.Persistence;
using IntegronERP.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.Modules.Inventory.Infrastructure.Persistence.Repositories;
using FluentValidation;
using IntegronERP.SharedKernel.Behaviors;

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

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);

            cfg.AddOpenBehavior(
                typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);

        services.AddScoped<IUnitOfWork, InventoryUnitOfWork>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductStockRepository,ProductStockRepository>();
        services.AddScoped<IStockMovementRepository,StockMovementRepository>();
        services.AddScoped<IWarehouseRepository,WarehouseRepository>();
        services.AddScoped<IWarehouseStockRepository,WarehouseStockRepository>();
        services.AddScoped<IWarehouseStockMovementRepository, WarehouseStockMovementRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        return services;
    }
}