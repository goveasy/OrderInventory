using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderInventory.Domain.Customers;
using OrderInventory.Domain.Orders;
using System;

namespace OrderInventory.Infrastructure.Persistence.Configurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Status)
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v))
            .IsRequired();

        builder.HasOne<Customer>().WithOne()
            .HasForeignKey<Order>(o => o.CustomerId)
            .IsRequired();

        builder.Navigation(o => o.OrderLines).AutoInclude();



    }
}
