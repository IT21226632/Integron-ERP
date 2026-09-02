namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;

public class UpdateSupplierStatusResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime UpdatedAt { get; set; }
}