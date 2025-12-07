using Microsoft.EntityFrameworkCore;
using OrderInventory.Application.Products;
using OrderInventory.Domain.Products;
using System;

namespace OrderInventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OrderInventoryDbContext _dbContext;

    public ProductRepository(OrderInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken)
    {
        return _dbContext.Products.FirstOrDefaultAsync(c => c.Id == productId, cancellationToken);
    }
}
