using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;

public class RegisterCompanyCommandHandler 
    : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUnitOfWork _unitOfWork;


    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _userManager = userManager;
        _roleManager = roleManager;
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



        // 4. Create Owner role if it does not exist

        const string ownerRole = "Owner";


        var roleExists = await _roleManager.RoleExistsAsync(ownerRole);


        if (!roleExists)
        {
            await _roleManager.CreateAsync(
                new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = ownerRole,
                    NormalizedName = ownerRole.ToUpper()
                });
        }



        // 5. Assign Owner role to user

        await _userManager.AddToRoleAsync(
            user,
            ownerRole);



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