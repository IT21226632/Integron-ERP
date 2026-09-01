namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;

public class GetWarehouseStockMovementsResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<WarehouseStockMovementDto> Items { get; set; }
        = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}