using IntegronERP.Modules.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegronERP.Modules.Inventory.Infrastructure.Configurations;

public class WarehouseStockConfiguration
    : IEntityTypeConfiguration<WarehouseStock>
{
    public void Configure(
        EntityTypeBuilder<WarehouseStock> builder)
    {
        builder.ToTable("WarehouseStocks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.WarehouseStocks)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.Stocks)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.ProductId,
            x.WarehouseId
        })
        .IsUnique();
    }
}