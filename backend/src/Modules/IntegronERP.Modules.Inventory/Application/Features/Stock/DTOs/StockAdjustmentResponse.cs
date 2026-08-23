namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class StockAdjustmentResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? ProductId { get; set; }

    public decimal? QuantityBefore { get; set; }

    public decimal? AdjustmentQuantity { get; set; }

    public decimal? QuantityAfter { get; set; }
}