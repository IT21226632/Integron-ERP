using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IntegronERP.Modules.Identity.Domain.Constants;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;

public class RegisterCompanyCommandHandler 
    : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;


    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }


    public async Task<RegisterCompanyResponse> Handle(
        RegisterCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;


        // 1. Check company already exists

        var companyExists = await _companyRepository.ExistsAsync(
            request.CompanyEmail,
            cancellationToken);


        if (companyExists)
        {
            return new RegisterCompanyResponse
            {
                Success = false,
                Message = "Company already exists."
            };
        }


        // 2. Create company

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.CompanyName,
            Email = request.CompanyEmail,
            PhoneNumber = request.CompanyPhone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };


        await _companyRepository.AddAsync(
            company,
            cancellationToken);


        // Save company first
        // because ApplicationUser has FK -> CompanyId

        await _unitOfWork.CommitAsync(
            cancellationToken);



        // 3. Create owner user

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CompanyId = company.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };


        var userResult = await _userManager.CreateAsync(
            user,
            request.Password);


        if (!userResult.Succeeded)
        {
            return new RegisterCompanyResponse
            {
                Success = false,
                Message = string.Join(
                    ", ",
                    userResult.Errors.Select(x => x.Description))
            };
        }

        // 5. Assign Owner role to user

        var roleResult = await _userManager.AddToRoleAsync(
        user,
        Roles.Owner);

    if (!roleResult.Succeeded)
    {
        return new RegisterCompanyResponse
        {
            Success = false,
            Message = string.Join(
                ", ",
                roleResult.Errors.Select(e => e.Description))
        };
    }



        // 6. Commit remaining changes

        await _unitOfWork.CommitAsync(
            cancellationToken);



        return new RegisterCompanyResponse
        {
            Success = true,
            Message = "Company registered successfully.",
            CompanyId = company.Id,
            UserId = user.Id
        };
    }
}