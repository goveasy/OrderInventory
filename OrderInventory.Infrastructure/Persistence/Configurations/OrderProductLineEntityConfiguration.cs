

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderInventory.Domain.Orders;
using OrderInventory.Domain.Products;

namespace OrderInventory.Infrastructure.Persistence.Configurations;

public class OrderProductLineEntityConfiguration : IEntityTypeConfiguration<OrderProductLine>
{
    public void Configure(EntityTypeBuilder<OrderProductLine> builder)
    {
        builder.ToTable("OrderProductLines");
        builder.HasKey(opl => new { opl.OrderId, opl.ProductId });
        builder.Property(opl => opl.Quantity).IsRequired();
        builder.Property(opl => opl.Reserved).IsRequired();


        builder.HasOne<Order>()
               .WithMany(c => c.OrderLines)
               .HasForeignKey(opl => opl.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>().WithMany()
               .HasForeignKey(opl => opl.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
