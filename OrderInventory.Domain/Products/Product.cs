using System;

namespace OrderInventory.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public ProductName Name { get; private set; }
    public decimal Price { get; private set; }
    public ProductStock Stock { get; private set; }

    public Product(Guid id, ProductName name, decimal price, ProductStock stock)
    {


        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }

    private Product()
    {
    }

    public void IncreaseStock(int amount)
    {
        Stock.Increase(amount);
    }

    public void DecreaseStock(int amount)
    {
        Stock.Decrease(amount);
    }
}
