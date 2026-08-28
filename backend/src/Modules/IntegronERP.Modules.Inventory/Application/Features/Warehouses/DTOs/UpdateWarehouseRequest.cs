namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class UpdateWarehouseRequest
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Address { get; set; }
}