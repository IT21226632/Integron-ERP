namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class ReserveStockRequest
{
    public decimal Quantity { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }
}