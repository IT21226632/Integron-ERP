using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using IntegronERP.Modules.Identity.Application.Services;
using IntegronERP.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)

    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // 2. Check account status
        if (!user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Your account has been deactivated."
            };
        }

        // 3. Verify password
        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // 4. Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // 5. Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(
            user,
            roles);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Revoked = false
        };

        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        // 6. Return response
        return new LoginResponse
        {
            Success = true,
            Message = "Login successful.",
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }
}