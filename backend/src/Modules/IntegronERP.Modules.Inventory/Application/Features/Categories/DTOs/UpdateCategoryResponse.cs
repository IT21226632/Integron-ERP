namespace IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;

public class UpdateCategoryResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public CategoryDto? Category { get; set; }
}