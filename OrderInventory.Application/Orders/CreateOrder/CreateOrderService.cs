using OrderInventory.Application.Abstractions;
using OrderInventory.Domain.Orders;


namespace OrderInventory.Application.Orders.CreateOrder;

public class CreateOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateOrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = Order.CreateNew(command.customerId);

        foreach (var line in command.Lines)
        {
            order.AddOrderLine(line.ProductId, line.Quantity);
        }

        _orderRepository.AddOrder(order);

        await _unitOfWork.CommitChangesAsync(cancellationToken);


        return new CreateOrderResult(order.Id, order.Status, command.Lines);
    }

}
