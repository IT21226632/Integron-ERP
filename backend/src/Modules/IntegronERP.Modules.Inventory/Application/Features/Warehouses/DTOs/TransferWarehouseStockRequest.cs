namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class TransferWarehouseStockRequest
{
    public Guid FromWarehouseId { get; set; }

    public Guid ToWarehouseId { get; set; }

    public decimal Quantity { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }
}