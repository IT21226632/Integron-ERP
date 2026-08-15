namespace IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;

public class CreateCategoryResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? CategoryId { get; set; }
}
