namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class ReserveStockResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? ProductId { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? ReservedQuantity { get; set; }

    public decimal? AvailableQuantity { get; set; }
}