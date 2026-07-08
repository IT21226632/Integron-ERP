using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;

public class RegisterCompanyCommand : IRequest<RegisterCompanyResponse>
{
    public RegisterCompanyRequest Request { get; }

    public RegisterCompanyCommand(RegisterCompanyRequest request)
    {
        Request = request;
    }
}