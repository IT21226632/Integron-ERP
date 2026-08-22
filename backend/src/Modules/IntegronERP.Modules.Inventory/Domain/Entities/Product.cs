namespace IntegronERP.Modules.Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Category Category { get; set; } = null!;
}