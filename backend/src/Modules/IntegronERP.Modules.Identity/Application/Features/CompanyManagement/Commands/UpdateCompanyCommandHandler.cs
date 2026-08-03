using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Commands;

public class UpdateCompanyCommandHandler
    : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCompanyResponse> Handle(
        UpdateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var company = await _companyRepository.GetByIdAsync(
            command.CompanyId,
            cancellationToken);

        if (company == null)
        {
            return new UpdateCompanyResponse
            {
                Success = false,
                Message = "Company not found."
            };
        }

        var existingCompany =
            await _companyRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

        if (existingCompany != null &&
            existingCompany.Id != company.Id)
        {
            return new UpdateCompanyResponse
            {
                Success = false,
                Message = "A company with this email already exists."
            };
        }

        company.Name = request.Name;
        company.Email = request.Email;
        company.PhoneNumber = request.PhoneNumber;

        await _companyRepository.UpdateAsync(
            company,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateCompanyResponse
        {
            Success = true,
            Message = "Company updated successfully."
        };
    }
}