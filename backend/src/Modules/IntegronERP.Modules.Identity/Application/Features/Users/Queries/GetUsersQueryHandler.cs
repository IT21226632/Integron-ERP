using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Queries;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        UserManager<Domain.Entities.ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<GetUsersResponse> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        // Get all users for the current company
        var users = await _userRepository.GetByCompanyAsync(
            request.CompanyId,
            cancellationToken);

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userDtos.Add(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? string.Empty,
                IsActive = user.IsActive
            });
        }

        return new GetUsersResponse
        {
            Success = true,
            Message = "Users retrieved successfully.",
            Users = userDtos
        };
    }
}


// For a small or medium ERP, this is perfectly acceptable while we're building features.

// Later, when we optimize performance, we'll replace it with a single SQL query that joins:

// AspNetUsers
// AspNetUserRoles
// AspNetRoles

// reducing it to one database call.