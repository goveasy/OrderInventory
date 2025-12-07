using Microsoft.EntityFrameworkCore;
using OrderInventory.Application.Orders;
using OrderInventory.Domain.Orders;
using System;

namespace OrderInventory.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderInventoryDbContext _dbContext;

        public OrderRepository(OrderInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
        }

        public Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return _dbContext.Orders.FirstOrDefaultAsync(c => c.Id == orderId, cancellationToken);
        }
    }
}
