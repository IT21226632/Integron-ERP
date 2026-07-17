using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using IntegronERP.Modules.Identity.Application.Services;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.Modules.Identity.Infrastructure.Configuration;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IOptions<JwtSettings> jwtOptions)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResponse> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Find refresh token
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (storedToken == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid refresh token."
            };
        }

        if (!storedToken.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Refresh token is no longer valid."
            };
        }

        // Load user
        var user = storedToken.User;

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Check user status
        if (!user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "User account is inactive."
            };
        }

        // Get roles
        var roles = await _userManager.GetRolesAsync(user);

        // Generate new tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(
            user,
            roles);

        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Revoke old refresh token
        storedToken.Revoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        await _refreshTokenRepository.UpdateAsync(
            storedToken,
            cancellationToken);

        // Save new refresh token
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            UserId = user.Id,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationDays),
            Revoked = false
        };

        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new LoginResponse
        {
            Success = true,
            Message = "Token refreshed successfully.",
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes)
        };
    }
}