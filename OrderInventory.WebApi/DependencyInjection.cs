using Medallion.Threading;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;
using OrderInventory.Application.Abstractions;
using OrderInventory.Application.Orders;
using OrderInventory.Application.Orders.ConfirmOrder;
using OrderInventory.Application.Orders.CreateOrder;
using OrderInventory.Application.Products;
using OrderInventory.Infrastructure.Persistence;
using OrderInventory.Infrastructure.Persistence.Repositories;

namespace OrderInventory.WebApi;


public static class DependencyInjection
{
    public static IServiceCollection AddSystemServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderInventoryDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("OrderInventoryDb"));
        });

        
        services.AddSingleton<IDistributedLockProvider>((sp) =>
        {
            return new PostgresDistributedSynchronizationProvider(configuration.GetConnectionString("OrderInventoryDb")!);
        });

        services.AddSingleton<IDataBaseLockService, DataBaseLockService>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrderInventoryDbContext>());
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<CreateOrderService>();
        services.AddScoped<ConfirmOrderService>();

        return services;
    }
}
