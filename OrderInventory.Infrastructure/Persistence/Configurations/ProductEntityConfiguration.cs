using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderInventory.Domain.Products;
using System;

namespace OrderInventory.Infrastructure.Persistence.Configurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => ProductName.Create(value))
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price).IsRequired();

        builder.Property(p => p.Stock)
            .HasConversion(
                name => name.Value,
                value => new ProductStock(value))
            .IsRequired();
    }
}
