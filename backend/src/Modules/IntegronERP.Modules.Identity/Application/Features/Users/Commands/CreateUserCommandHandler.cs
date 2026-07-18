using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public CreateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
{
    _userManager = userManager;
    _roleManager = roleManager;
}

    public async Task<CreateUserResponse> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Check email already exists
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = "A user with this email already exists."
            };
        }

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = $"Role '{request.Role}' does not exist."
            };
        }

        // Create user
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),

            UserName = request.Email,
            Email = request.Email,

            FirstName = request.FirstName,
            LastName = request.LastName,

            CompanyId = command.CompanyId,

            EmailConfirmed = true,
            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description))
            };
        }

        // Assign role
        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                request.Role);

        if (!roleResult.Succeeded)
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = string.Join(
                    ", ",
                    roleResult.Errors.Select(x => x.Description))
            };
        }

        return new CreateUserResponse
        {
            Success = true,
            Message = "User created successfully.",

            UserId = user.Id,
            Email = user.Email!,
            Role = request.Role
        };
    }
}