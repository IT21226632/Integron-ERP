namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class WarehouseStockDto
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal ReservedQuantity { get; set; }

    public decimal AvailableQuantity { get; set; }

    public DateTime UpdatedAt { get; set; }
}