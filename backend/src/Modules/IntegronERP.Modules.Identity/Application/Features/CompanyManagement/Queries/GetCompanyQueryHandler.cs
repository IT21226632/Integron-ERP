using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Queries;

public class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, GetCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyQueryHandler(
        ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<GetCompanyResponse> Handle(
        GetCompanyQuery query,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            query.CompanyId,
            cancellationToken);

        if (company == null)
        {
            return new GetCompanyResponse
            {
                Success = false,
                Message = "Company not found."
            };
        }

        return new GetCompanyResponse
        {
            Success = true,
            Message = "Company retrieved successfully.",

            Id = company.Id,
            Name = company.Name,
            Email = company.Email,
            PhoneNumber = company.PhoneNumber,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt
        };
    }
}