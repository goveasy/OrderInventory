using OrderInventory.Application.Abstractions;
using OrderInventory.Application.Products;
using OrderInventory.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderInventory.Application.Orders.ConfirmOrder;

public class ConfirmOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IDataBaseLockService _dataBaseLockService;

    public ConfirmOrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IDataBaseLockService dataBaseLockService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _dataBaseLockService = dataBaseLockService;
    }

    public async Task ConfirmOrderAsync(ConfirmOrderCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken)
                    ?? throw new InvalidOperationException("Order not found.");

        if (!order.CanConfirm())
        {
            throw new InvalidOperationException("Order cannot be confirmed in its current state.");
        }

        var productLocks = await AcquireProductLocksAsync(order, cancellationToken);

        try
        {
            await _unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                await ReserveStock(order, ct);

                order.ConfirmOrder();

                await _unitOfWork.CommitChangesAsync(ct);
            }, cancellationToken);
        }
        finally
        {
            await ReleaseLocksAsync(productLocks);
        }
    }

    private async Task ReserveStock(Order order, CancellationToken cancellationToken)
    {
        foreach (var line in order.OrderLines)
        {
            var product = await _productRepository.GetAsync(line.ProductId, cancellationToken)
                          ?? throw new InvalidOperationException("Product not found.");

            product.DecreaseStock(line.Quantity);
            line.MarkAsReserved();
        }
    }

    private async Task<IReadOnlyCollection<IAsyncDisposable>> AcquireProductLocksAsync(Order order, CancellationToken cancellationToken)
    {
        var locks = new List<IAsyncDisposable>();

        try
        {
            foreach (var productId in order.OrderLines.Select(line => line.ProductId).Distinct().OrderBy(id => id))
            {
                var productLock = await _dataBaseLockService.AcquireLockAsync(productId.ToString(), TimeSpan.FromSeconds(5), cancellationToken);

                if (productLock is null)
                {
                    throw new InvalidOperationException($"Could not acquire lock for product {productId}");
                }

                locks.Add(productLock);
            }

            return locks;
        }
        catch
        {
            await ReleaseLocksAsync(locks);
            throw;
        }
    }

    private static async Task ReleaseLocksAsync(IEnumerable<IAsyncDisposable> locks)
    {
        foreach (var productLock in locks)
        {
            await productLock.DisposeAsync();
        }
    }
}
