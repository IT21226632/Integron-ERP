namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;

public class RegisterCompanyRequest
{
    // Company Information
    public string CompanyName { get; set; } = string.Empty;

    public string CompanyEmail { get; set; } = string.Empty;

    public string CompanyPhone { get; set; } = string.Empty;

    // Owner Information
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}