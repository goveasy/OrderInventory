

using System;

namespace OrderInventory.Domain.Customers;

public sealed class CustomerName
{
    public string Value { get; private set; }

    public CustomerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Customer name cannot be null or empty.", nameof(value));
        }
        Value = value;
    }
}
