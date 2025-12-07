using System;
using System.Collections.Generic;
using System.Linq;
namespace OrderInventory.Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public OrderStatus Status { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderProductLine> OrderLines { get; init; } = new List<OrderProductLine>();


    private Order(Guid id, OrderStatus status, Guid customerId)
    {
        Id = id;
        Status = status;
        CustomerId = customerId;
    }

    private Order()
    {
    }

    public static Order CreateNew(Guid customerId)
    {
        return new Order(Guid.NewGuid(), OrderStatus.Draft, customerId);
    }

    public void AddOrderLine(Guid productId, int quantity)
    {
        CheckIfCanUpdateLines();

        var currentLine = OrderLines.FirstOrDefault(c => c.ProductId == productId);
        if (currentLine is not null)
        {
            currentLine.UpdateQuantity(currentLine.Quantity + quantity);
        }
        else
        {
            var newLine = new OrderProductLine(this.Id,productId, quantity);
            OrderLines.Add(newLine);
        }
    }


    public void RemoveOrderLine(Guid productId, int quantity)
    {
        CheckIfCanUpdateLines();

        var currentLine = OrderLines.FirstOrDefault(c => c.ProductId == productId) ?? throw new InvalidOperationException("Order line for the specified product does not exist.");

        if (currentLine.Quantity < quantity)
        {
            throw new InvalidOperationException("Cannot remove more items than are present in the order line.");
        }

        var newQuantity = currentLine.Quantity - quantity;
        
        if (newQuantity == 0)
        {
            OrderLines.Remove(currentLine);
        }
        else
        {
            currentLine.UpdateQuantity(newQuantity);
        }
    }

    private void CheckIfCanUpdateLines()
    {
        var updatableStatuses = new[] { OrderStatus.Confirmed, OrderStatus.Paid, OrderStatus.Cancelled};
        
        if (updatableStatuses.Contains(Status))
        {
            throw new InvalidOperationException($"Cannot update order lines when order status is {Status}.");
        }
    }

    public void ConfirmOrder()
    {
        if(Status != OrderStatus.Draft)
        {
            throw new InvalidOperationException("Only draft orders can be confirmed.");
        }

        if (OrderLines.Count == 0)
        {
            throw new InvalidOperationException("Cannot confirm an order with no order lines.");
        }

        Status = OrderStatus.Confirmed;

    }

    public void PayOrder()
    {
        if(Status != OrderStatus.Confirmed)
        {
            throw new InvalidOperationException("Only confirmed orders can be paid.");
        }
        Status = OrderStatus.Paid;
    }

    public void CancelOrder()
    {
        if(Status == OrderStatus.Paid)
        {
            throw new InvalidOperationException("Paid orders cannot be cancelled.");
        }
        Status = OrderStatus.Cancelled;
    }

    public bool CanConfirm()
    {
        return Status == OrderStatus.Draft && OrderLines.Count > 0;
    }



}
