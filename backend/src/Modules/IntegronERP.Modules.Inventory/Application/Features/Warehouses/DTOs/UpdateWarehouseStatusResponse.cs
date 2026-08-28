namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class UpdateWarehouseStatusResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? WarehouseId { get; set; }

    public bool? IsActive { get; set; }
}