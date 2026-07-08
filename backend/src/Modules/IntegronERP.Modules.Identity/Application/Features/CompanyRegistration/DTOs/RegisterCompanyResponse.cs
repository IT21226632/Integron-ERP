namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;

public class RegisterCompanyResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? CompanyId { get; set; }

    public Guid? UserId { get; set; }
}