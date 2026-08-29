namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class ProductStockDto
{
    public Guid ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal AllocatedQuantity { get; set; }

    public decimal UnallocatedQuantity { get; set; }

    public decimal ReservedQuantity { get; set; }

    public decimal AvailableQuantity { get; set; }

    public DateTime UpdatedAt { get; set; }
}