namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class TransferWarehouseStockResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid ProductId { get; set; }

    public Guid FromWarehouseId { get; set; }

    public Guid ToWarehouseId { get; set; }

    public decimal TransferredQuantity { get; set; }

    public decimal FromWarehouseQuantity { get; set; }

    public decimal ToWarehouseQuantity { get; set; }
}