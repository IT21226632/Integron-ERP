namespace IntegronERP.Modules.Inventory.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<WarehouseStock> Stocks { get; set; }
        = new List<WarehouseStock>();
}