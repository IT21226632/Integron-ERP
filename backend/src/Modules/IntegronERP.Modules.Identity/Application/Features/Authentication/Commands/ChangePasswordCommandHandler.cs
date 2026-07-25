using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using IntegronERP.Modules.Identity.Domain.Entities;
using IntegronERP.Modules.Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using IntegronERP.SharedKernel.Interfaces;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChangePasswordResponse> Handle(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Find user

        var user = await _userManager.FindByIdAsync(
            command.UserId.ToString());

        if (user == null)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Prevent using the same password

        var samePassword = await _userManager.CheckPasswordAsync(
            user,
            request.NewPassword);

        if (samePassword)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = "New password must be different from the current password."
            };
        }

        // Change password

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
        {
            return new ChangePasswordResponse
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

        foreach (var token in refreshTokens.Where(t => !t.Revoked))
        {
            token.Revoked = true;
            token.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(
                token,
                cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ChangePasswordResponse
        {
            Success = true,
            Message = "Password changed successfully."
        };
    }
}