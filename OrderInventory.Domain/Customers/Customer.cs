using System;

namespace OrderInventory.Domain.Customers;

public sealed class Customer
{
   
    public Guid Id { get; private set; }
    public CustomerName Name { get; private set; }
    public Customer(Guid id, CustomerName name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    private Customer()
    {
    }
}
