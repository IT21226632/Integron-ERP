namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class ReturnWarehouseStockResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal ReturnedQuantity { get; set; }

    public decimal WarehouseQuantity { get; set; }

    public decimal WarehouseAvailableQuantity { get; set; }

    public decimal TotalProductStock { get; set; }

    public decimal AllocatedQuantity { get; set; }

    public decimal UnallocatedQuantity { get; set; }
}