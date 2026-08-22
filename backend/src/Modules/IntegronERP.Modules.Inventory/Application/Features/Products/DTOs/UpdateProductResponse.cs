namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class UpdateProductResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? ProductId { get; set; }
}