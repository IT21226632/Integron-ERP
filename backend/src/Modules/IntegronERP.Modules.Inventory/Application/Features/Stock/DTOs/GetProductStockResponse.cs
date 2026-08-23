namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class GetProductStockResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public ProductStockDto? Stock { get; set; }
}