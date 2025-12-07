using System;
namespace OrderInventory.Domain.Products;

public sealed class ProductStock
{
    public int Value { get; private set; }

    public ProductStock(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Stock value cannot be negative.", nameof(value));
        }
        Value = value;
    }

    private ProductStock()
    {

    }

    public void Decrease(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Decrease amount cannot be negative.", nameof(amount));
        }
        if (amount > Value)
        {
            throw new InvalidOperationException("Insufficient stock to decrease by the specified amount.");
        }

        Value -= amount;
    }

    public void Increase(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Increase amount cannot be negative.", nameof(amount));
        }
        Value += amount;
    }

}
