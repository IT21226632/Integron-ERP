namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}