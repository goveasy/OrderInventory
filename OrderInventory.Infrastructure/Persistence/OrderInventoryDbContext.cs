

using Microsoft.EntityFrameworkCore;
using OrderInventory.Application.Abstractions;
using OrderInventory.Domain.Orders;
using OrderInventory.Domain.Products;

namespace OrderInventory.Infrastructure.Persistence;

public class OrderInventoryDbContext : DbContext, IUnitOfWork 
{

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();

    public OrderInventoryDbContext(DbContextOptions<OrderInventoryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderInventoryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }
}
