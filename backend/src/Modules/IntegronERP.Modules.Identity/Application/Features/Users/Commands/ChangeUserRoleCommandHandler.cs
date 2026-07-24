using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public class ChangeUserRoleCommandHandler
    : IRequestHandler<ChangeUserRoleCommand, ChangeUserRoleResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public ChangeUserRoleCommandHandler(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ChangeUserRoleResponse> Handle(
        ChangeUserRoleCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;


        // Prevent changing your own role

        if (command.UserId == command.CurrentUserId)
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = "You cannot change your own role."
            };
        }

        // Find user

        var user = await _userRepository.GetByIdAsync(
            command.UserId,
            cancellationToken);

        if (user == null)
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Tenant protection

        if (user.CompanyId != command.CompanyId)
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Validate role exists

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = $"Role '{request.Role}' does not exist."
            };
        }


        // Prevent removing the last Owner

        var currentRoles = await _userManager.GetRolesAsync(user);

        var isOwner =
            currentRoles.Contains("Owner");

        if (isOwner &&
            request.Role != "Owner")
        {
            var ownerCount =
                await _userRepository.GetActiveOwnerCountAsync(
                    command.CompanyId,
                    cancellationToken);

            if (ownerCount <= 1)
            {
                return new ChangeUserRoleResponse
                {
                    Success = false,
                    Message =
                        "A company must have at least one active Owner."
                };
            }
        }

        // Remove existing roles

        if (currentRoles.Any())
        {
            var removeResult =
                await _userManager.RemoveFromRolesAsync(
                    user,
                    currentRoles);

            if (!removeResult.Succeeded)
            {
                return new ChangeUserRoleResponse
                {
                    Success = false,
                    Message = string.Join(
                        ", ",
                        removeResult.Errors.Select(e => e.Description))
                };
            }
        }

        // Add new role

        var addResult = await _userManager.AddToRoleAsync(
            user,
            request.Role);

        if (!addResult.Succeeded)
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = string.Join(
                    ", ",
                    addResult.Errors.Select(e => e.Description))
            };
        }

        return new ChangeUserRoleResponse
        {
            Success = true,
            Message = "User role updated successfully."
        };
    }
}