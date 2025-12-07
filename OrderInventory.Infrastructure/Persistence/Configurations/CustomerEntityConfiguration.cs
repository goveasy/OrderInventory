

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderInventory.Domain.Customers;

namespace OrderInventory.Infrastructure.Persistence.Configurations;

public class CustomerEntityConfiguration: IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .HasConversion(
                v => v.Value,
                v => new CustomerName(v))
            .IsRequired();
    }

}
