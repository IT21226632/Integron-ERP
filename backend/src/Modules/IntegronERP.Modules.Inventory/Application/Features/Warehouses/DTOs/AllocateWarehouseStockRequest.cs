namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class AllocateWarehouseStockRequest
{
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }
}