namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class GetWarehouseByIdResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public WarehouseDto? Warehouse { get; set; }
}