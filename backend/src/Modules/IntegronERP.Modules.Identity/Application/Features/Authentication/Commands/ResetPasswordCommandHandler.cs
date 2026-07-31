using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResetPasswordResponse> Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Prevent resetting your own password

        if (command.UserId == command.CurrentUserId)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = "Use Change Password to update your own password."
            };
        }

        // Find target user

        var user = await _userRepository.GetByIdAsync(
            command.UserId,
            cancellationToken);

        if (user == null)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Tenant protection

        if (user.CompanyId != command.CompanyId)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Reset password

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            request.NewPassword);

        if (!result.Succeeded)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description))
            };
        }

        // Revoke all refresh tokens

        var refreshTokens =
            await _refreshTokenRepository.GetByUserIdAsync(
                user.Id,
                cancellationToken);

        foreach (var refreshToken in refreshTokens.Where(x => !x.Revoked))
        {
            refreshToken.Revoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(
                refreshToken,
                cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResetPasswordResponse
        {
            Success = true,
            Message = "Password reset successfully."
        };
    }
}