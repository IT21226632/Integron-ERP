namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class GetWarehousesResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<WarehouseDto> Warehouses { get; set; }
        = new();
}