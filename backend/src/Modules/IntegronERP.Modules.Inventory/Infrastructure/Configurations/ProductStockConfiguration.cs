using IntegronERP.Modules.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegronERP.Modules.Inventory.Infrastructure.Configurations;

public class ProductStockConfiguration
    : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(
        EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("ProductStocks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.ProductId
        })
        .IsUnique();

        builder.HasOne(x => x.Product)
            .WithOne(x => x.Stock)
            .HasForeignKey<ProductStock>(
                x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}