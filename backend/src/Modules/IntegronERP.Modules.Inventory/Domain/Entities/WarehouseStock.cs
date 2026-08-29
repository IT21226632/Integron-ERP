namespace IntegronERP.Modules.Inventory.Domain.Entities;

public class WarehouseStock
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }

    public decimal ReservedQuantity { get; set; }

    public decimal AvailableQuantity =>
        Quantity - ReservedQuantity;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Product Product { get; set; } = null!;

    public Warehouse Warehouse { get; set; } = null!;
}