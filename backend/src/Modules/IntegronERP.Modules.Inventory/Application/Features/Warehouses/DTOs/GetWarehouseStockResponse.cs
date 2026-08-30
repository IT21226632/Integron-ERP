namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class GetWarehouseStockResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid WarehouseId { get; set; }

    public string WarehouseName { get; set; } = string.Empty;

    public List<WarehouseStockDto> Stocks { get; set; }
        = new();
}