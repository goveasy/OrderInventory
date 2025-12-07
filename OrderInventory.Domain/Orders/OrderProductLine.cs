

using System;

namespace OrderInventory.Domain.Orders;

public class OrderProductLine
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public bool Reserved { get; private set; }

    public OrderProductLine(Guid orderId, Guid productId, int quantity)
    {
       
        UpdateQuantity(quantity);

        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Reserved = false;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
        }
        Quantity = newQuantity;
    }

    public void MarkAsReserved()
    {
        Reserved = true;
    }

    public void MarkAsUnreserved()
    {
        Reserved = false;
    }
}
