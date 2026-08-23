using IntegronERP.Modules.Inventory.Domain.Constants;

namespace IntegronERP.Modules.Inventory.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid ProductId { get; set; }

    public StockMovementType MovementType { get; set; }

    public decimal Quantity { get; set; }

    public decimal QuantityBefore { get; set; }

    public decimal QuantityAfter { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public Product Product { get; set; } = null!;
}