using System;

namespace OrderInventory.Domain.Products;

public sealed class ProductName
{
    public string Value { get; private set; }

    private ProductName(string value)
    {
        Value = value;
    }

    private ProductName()
    {
    }

    public static ProductName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product name cannot be empty.");
        }
        if (value.Length > 100)
        {
            throw new ArgumentException("Product name cannot exceed 100 characters.");
        }
        return new ProductName(value);
    }
}
