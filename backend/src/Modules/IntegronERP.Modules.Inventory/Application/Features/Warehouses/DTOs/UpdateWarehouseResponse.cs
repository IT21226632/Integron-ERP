namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class UpdateWarehouseResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? WarehouseId { get; set; }
}