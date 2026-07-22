using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IntegronERP.Modules.Identity.Domain.Entities;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Queries;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;


    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }


    public async Task<GetUserByIdResponse> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            query.UserId,
            cancellationToken);


        if (user == null)
        {
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User not found."
            };
        }


        // Multi tenant protection
        if (user.CompanyId != query.CompanyId)
        {
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User not found."
            };
        }


        var roles = await _userManager.GetRolesAsync(user);


        return new GetUserByIdResponse
        {
            Success = true,
            Message = "User retrieved successfully.",
            User = new UserDetailsDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            }
        };
    }
}