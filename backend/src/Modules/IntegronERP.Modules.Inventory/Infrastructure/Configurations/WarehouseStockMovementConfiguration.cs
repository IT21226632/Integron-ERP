using IntegronERP.Modules.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegronERP.Modules.Inventory.Infrastructure.Persistence.Configurations;

public class WarehouseStockMovementConfiguration
    : IEntityTypeConfiguration<WarehouseStockMovement>
{
    public void Configure(
        EntityTypeBuilder<WarehouseStockMovement> builder)
    {
        builder.ToTable("WarehouseStockMovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 2);

        builder.Property(x => x.QuantityBefore)
            .HasPrecision(18, 2);

        builder.Property(x => x.QuantityAfter)
            .HasPrecision(18, 2);

        builder.Property(x => x.Reference)
            .HasMaxLength(200);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.MovementType)
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.WarehouseId,
            x.CreatedAt
        });
    }
}