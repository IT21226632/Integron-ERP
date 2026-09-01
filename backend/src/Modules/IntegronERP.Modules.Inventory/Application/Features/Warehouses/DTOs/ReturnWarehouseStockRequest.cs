namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class ReturnWarehouseStockRequest
{
    public decimal Quantity { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }
}