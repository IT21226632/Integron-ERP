namespace IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;

public class GetCategoriesResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<CategoryDto> Categories { get; set; } = new();
}