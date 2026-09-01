namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class WarehouseStockMovementDto
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public string MovementType { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal QuantityBefore { get; set; }

    public decimal QuantityAfter { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}