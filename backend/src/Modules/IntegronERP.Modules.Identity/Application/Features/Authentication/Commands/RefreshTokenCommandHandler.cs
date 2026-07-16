using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using IntegronERP.Modules.Identity.Application.Services;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
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
    RefreshTokenCommand command,
    CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. Find the refresh token
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        // 2. Validate - token exists
        if (storedToken == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid refresh token."
            };
        }

        // 3. Validate - token not revoked
        if (storedToken.Revoked)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Refresh token has been revoked."
            };
        }

        // 4. Validate - token not expired
        if (storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Refresh token has expired."
            };
        }

        // Temporary return (we'll replace this in the next step)
        return new LoginResponse
        {
            Success = false,
            Message = "Refresh token processing is not implemented yet."
        };
    }
}