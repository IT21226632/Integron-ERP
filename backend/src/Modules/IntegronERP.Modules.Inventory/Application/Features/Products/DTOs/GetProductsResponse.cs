namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class GetProductsResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<ProductDto> Products { get; set; } = new();
}