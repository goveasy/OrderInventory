using OrderInventory.Domain.Orders;

namespace OrderInventory.Application.Orders;

public interface IOrderRepository
{
    public Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
    public void AddOrder(Order order);

}
