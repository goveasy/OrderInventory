using OrderInventory.Domain.Orders;
using OrderInventory.Domain.Products;

namespace OrderInventory.Application.Products
{
    public interface IProductRepository
    {
        public Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken);
       
    }
}
