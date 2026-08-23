namespace IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

public class UpdateProductStatusResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? ProductId { get; set; }

    public bool? IsActive { get; set; }
}