using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;

public class RegisterCompanyCommandHandler 
    : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;


    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _companyRepository = companyRepository;
        _userManager = userManager;
        _roleManager = roleManager;
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
            PhoneNumber = request.CompanyPhone
        };


        await _companyRepository.AddAsync(
            company,
            cancellationToken);



        // 3. Create owner user

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CompanyId = company.Id
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



        // 4. Create Owner role

        const string ownerRole = "Owner";


        if (!await _roleManager.RoleExistsAsync(ownerRole))
        {
            await _roleManager.CreateAsync(
                new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = ownerRole,
                    NormalizedName = ownerRole.ToUpper()
                });
        }



        // 5. Assign role

        await _userManager.AddToRoleAsync(
            user,
            ownerRole);



        // 6. Save company

        await _companyRepository.SaveChangesAsync(
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