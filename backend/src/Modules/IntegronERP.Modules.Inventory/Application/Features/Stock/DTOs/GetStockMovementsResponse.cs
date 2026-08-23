namespace IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;

public class GetStockMovementsResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<StockMovementDto> Movements { get; set; } = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages =>
        PageSize > 0
            ? (int)Math.Ceiling(
                (double)TotalCount / PageSize)
            : 0;
}