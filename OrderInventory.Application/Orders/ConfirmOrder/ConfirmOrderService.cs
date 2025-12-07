using OrderInventory.Application.Abstractions;
using OrderInventory.Application.Products;
using OrderInventory.Domain.Orders;
using System;

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


        foreach (var line in order.OrderLines)
        {
            try
            {
                await ReserveStock(line, cancellationToken);
            }
            catch (Exception)
            {
                foreach (var reservedLine in order.OrderLines.Where(l => l.Reserved))
                {
                    await RollbackReservation(reservedLine, cancellationToken);
                }
                throw;
            }
        }

        order.ConfirmOrder();

        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    private async Task ReserveStock(OrderProductLine line, CancellationToken cancellationToken)
    {
        var productLock = await _dataBaseLockService.AcquireLockAsync(line.ProductId.ToString(), TimeSpan.FromSeconds(5), cancellationToken);

        if (productLock is null)
        {
            throw new InvalidOperationException($"Could not acquire lock for product {line.ProductId}");
        }

        try
        {
            var product = await _productRepository.GetAsync(line.ProductId, cancellationToken)
                          ?? throw new InvalidOperationException("Product not found.");

            product.DecreaseStock(line.Quantity);
            line.MarkAsReserved();

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
        finally
        {
            await productLock.DisposeAsync();
        }
    }

    private async Task RollbackReservation(OrderProductLine line, CancellationToken cancellationToken)
    {
        var productLock = await _dataBaseLockService.AcquireLockAsync(line.ProductId.ToString(), TimeSpan.FromSeconds(5), cancellationToken);
        if (productLock is null)
        {
            throw new InvalidOperationException($"Could not acquire lock for product {line.ProductId}");
        }
        try
        {
            var product = await _productRepository.GetAsync(line.ProductId, cancellationToken)
                          ?? throw new InvalidOperationException("Product not found.");
            product.IncreaseStock(line.Quantity);
            line.MarkAsUnreserved();
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
        finally
        {
            await productLock.DisposeAsync();
        }
    }


}
