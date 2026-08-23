namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class GetStockMovementsResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<StockMovementDto> Movements { get; set; } = new();
}