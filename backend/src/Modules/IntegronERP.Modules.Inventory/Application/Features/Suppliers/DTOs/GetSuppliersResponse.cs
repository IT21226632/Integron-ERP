namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;

public class GetSuppliersResponse
{
    public List<SupplierDto> Suppliers { get; set; } = new();
}

public class SupplierDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ContactPerson { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}