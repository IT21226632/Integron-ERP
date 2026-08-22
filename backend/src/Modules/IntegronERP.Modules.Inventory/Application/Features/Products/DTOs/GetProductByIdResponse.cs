namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class GetProductByIdResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public ProductDto? Product { get; set; }
}