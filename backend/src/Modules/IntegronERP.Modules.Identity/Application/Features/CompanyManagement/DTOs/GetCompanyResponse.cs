namespace IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;

public class GetCompanyResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}