namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class UpdateProductRequest
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }
}